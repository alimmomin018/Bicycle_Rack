using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BicycleRack.Model
{
    public class RackLocation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Capacity { get; set; }
        public string MultiModal { get; set; }
        public string Seasonal { get; set; }
        public string Sheltered { get; set; }
        public string Surface { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string MapClass { get; set; }
        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible == value)
                    return;

                isVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsVisible)));
            }
        }
    }
}
