using Sangkay.Domain.Entities;
using Sangkay.Framework.Interfaces;
using Sangkay.Supplier.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System;
using System.Threading.Tasks;

namespace Sangkay.Supplier
{
    public class SupplierViewModel : INotifyPropertyChanged
    {
        private readonly ISupplierRepository _repo;

        // Store domain entities (fully-qualified to avoid ambiguity)
        public ObservableCollection<Sangkay.Domain.Entities.Supplier> Suppliers { get; } = new();

        private Sangkay.Domain.Entities.Supplier? _selectedSupplier;
        public Sangkay.Domain.Entities.Supplier? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (ReferenceEquals(_selectedSupplier, value)) return;
                _selectedSupplier = value;
                OnPropertyChanged();
                PopulateFieldsFromSelected();
                AddCommand.RaiseCanExecuteChanged();
                UpdateCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        private string _address = "";
        public string Address
        {
            get => _address;
            set { if (_address == value) return; _address = value; OnPropertyChanged(); }
        }

        private string _contact = "";
        public string Contact
        {
            get => _contact;
            set { if (_contact == value) return; _contact = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand NewCommand { get; }

        public SupplierViewModel(ISupplierRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            // Cast async lambdas to Func<object?, Task> to pick the async constructor overload reliably
            AddCommand = new RelayCommand((Func<object?, Task>)(async _ => await AddAsync()), _ => !string.IsNullOrWhiteSpace(Name));
            UpdateCommand = new RelayCommand((Func<object?, Task>)(async _ => await UpdateAsync()), _ => SelectedSupplier != null);
            DeleteCommand = new RelayCommand((Func<object?, Task>)(async _ => await DeleteAsync()), _ => SelectedSupplier != null);
            NewCommand = new RelayCommand(_ => ClearFields());

            _ = LoadAsync();
        }

        private void PopulateFieldsFromSelected()
        {
            if (SelectedSupplier == null)
            {
                ClearFields(true);
                return;
            }

            Name = SelectedSupplier.Name;
            Address = SelectedSupplier.Address;
            Contact = SelectedSupplier.Contact;
        }

        public async Task LoadAsync()
        {
            Suppliers.Clear();
            var list = await _repo.GetAllAsync();
            foreach (var s in list) Suppliers.Add(s);

            AddCommand.RaiseCanExecuteChanged();
            UpdateCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        public async Task AddAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Supplier name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var s = new Sangkay.Domain.Entities.Supplier
                {
                    Name = Name.Trim(),
                    Address = Address.Trim(),
                    Contact = Contact.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(s);
                await _repo.SaveChangesAsync();

                Application.Current?.Dispatcher.Invoke(() => Suppliers.Add(s));
                ClearFields();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AddAsync failed: " + ex);
                MessageBox.Show($"Failed to add supplier:\n\n{ex.Message}", "Add error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task UpdateAsync()
        {
            if (SelectedSupplier == null) return;

            SelectedSupplier.Name = Name.Trim();
            SelectedSupplier.Address = Address.Trim();
            SelectedSupplier.Contact = Contact.Trim();
            SelectedSupplier.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _repo.UpdateAsync(SelectedSupplier);
                await _repo.SaveChangesAsync();
                await LoadAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UpdateAsync failed: " + ex);
                MessageBox.Show($"Failed to update supplier:\n\n{ex.Message}", "Update error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task DeleteAsync()
        {
            if (SelectedSupplier == null) return;

            var confirm = MessageBox.Show($"Delete '{SelectedSupplier.Name}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                await _repo.SaveChangesAsync();

                Application.Current?.Dispatcher.Invoke(() => Suppliers.Remove(SelectedSupplier));
                ClearFields();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DeleteAsync failed: " + ex);
                MessageBox.Show(ex.Message, "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFields(bool keepSelected = false)
        {
            if (!keepSelected) SelectedSupplier = null;
            Name = "";
            Address = "";
            Contact = "";

            AddCommand.RaiseCanExecuteChanged();
            UpdateCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}