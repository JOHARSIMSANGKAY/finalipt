using Sangkay.Domain.Entities;
using System.Diagnostics;
using System.Windows.Input;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Sangkay.Supplier.Helpers
{
    // Single RelayCommand that supports both sync Action<object?> and async Func<object?, Task>
    // and exposes RaiseCanExecuteChanged. Prevents reentrancy while async handler runs.
    public class RelayCommand : ICommand
    {
        private readonly Action<object?>? _executeSync;
        private readonly Func<object?, Task>? _executeAsync;
        private readonly Predicate<object?>? _canExecute;
        private bool _isExecuting;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _executeSync = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Func<object?, Task> executeAsync, Predicate<object?>? canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);

        public async void Execute(object? parameter)
        {
            if (_executeAsync != null)
            {
                await ExecuteAsync(parameter).ConfigureAwait(false);
                return;
            }

            _executeSync?.Invoke(parameter);
        }

        public async Task ExecuteAsync(object? parameter)
        {
            if (!CanExecute(parameter)) return;

            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();
                await _executeAsync!(parameter).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Do not rethrow on UI thread. Log and show the real exception instead.
                Debug.WriteLine("Command execution failed: " + ex.ToString());
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Command execution failed:\n\n{ex.Message}", "Command error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                _localCanExecuteChanged += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                _localCanExecuteChanged -= value;
            }
        }

        private event EventHandler? _localCanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (Application.Current == null)
            {
                _localCanExecuteChanged?.Invoke(this, EventArgs.Empty);
                CommandManager.InvalidateRequerySuggested();
                return;
            }

            if (Application.Current.Dispatcher.CheckAccess())
            {
                _localCanExecuteChanged?.Invoke(this, EventArgs.Empty);
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _localCanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    CommandManager.InvalidateRequerySuggested();
                });
            }
        }

    }
}