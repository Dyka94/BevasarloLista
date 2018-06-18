using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BevasarloLista.Behaviors
{
    class ListViewNoSelectionBehavior : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView listView)
        {
            listView.ItemSelected += OnItemSelected;
            base.OnAttachedTo(listView);
        }
        protected override void OnDetachingFrom(ListView listView)
        {
            listView.ItemSelected -= OnItemSelected;
            base.OnDetachingFrom(listView);
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
