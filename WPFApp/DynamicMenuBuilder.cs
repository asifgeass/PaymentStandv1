using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFApp.ViewModels;

namespace WPFApp
{
    public class DynamicMenuBuilder
    {
        #region fields and ctor
        DynamicMenuWindowViewModel model;
        Window window;
        FormAround form = new FormAround();
        public DynamicMenuBuilder(Window incWindow)
        {
            window = incWindow;
            try
            {
                model = window.DataContext as DynamicMenuWindowViewModel;
                model.ResponseChangedEvent += BuildMenuOnReponse;
                model.PropertyChanged += IsLoadingMenuChanged;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void IsLoadingMenuChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsLoadingMenu")
            {
                window.Content = null;
            }
        }
        #endregion

        private void BuildMenuOnReponse()
        {
            if (model == null) return;
            var rootResponse = model.Responce;
            var resp = rootResponse.GetListResponse;
            var paylist = resp.PayRecord;
            if (paylist.Count > 1)
            {
                foreach (var payrec in paylist)
                {
                    CreateButton(payrec);
                }
            }
            if (paylist.Count == 1)
            {
                var attrRecords = paylist.First().AttrRecord;

                foreach (var it in attrRecords)
                {
                    if (it.Edit == 1 && it.View == 1 && !string.IsNullOrEmpty(it.Lookup))
                    {
                        var lookups = paylist.First().Lookups;
                        var selectedLookup = lookups.Where(x => x.Name.ToLower() == it.Lookup.ToLower()).Single();
                        form.Controls.Add(new Label()
                        {
                            Content = selectedLookup.Name,
                            HorizontalContentAlignment = HorizontalAlignment.Center
                        });
                        var lookItems = selectedLookup.Item;
                        lookItems.ForEach(x =>
                        {
                            form.Controls.Add(new Button()
                            {
                                Content = $"{x.Value}",
                            });
                            form.Controls.Add(new Label());
                        });
                        new Window() { Content = form, Width = 450, Height = 800 }.Show();
                        form = new FormAround();
                    }
                }

                foreach (var it in attrRecords)
                {
                    if (it.Edit != 1 && it.View == 1)
                    {
                        var label = new Label();
                        label.Content = $"{it.Name} = {it.Value}";

                        form.Controls.Add(label);
                        form.Controls.Add(new Label());
                    }
                }
                foreach (var it in attrRecords)
                {
                    if (it.Edit == 1 && it.View == 1 && string.IsNullOrEmpty(it.Lookup))
                    {
                        var label = new Label();
                        label.Content = $"{it.Name}:";
                        var inputbox = new TextBox();

                        form.Controls.Add(label);
                        form.Controls.Add(inputbox);
                        form.Controls.Add(new Label());
                    }
                }

                form.Controls.Add(new Label());
                form.Controls.Add(new Label());
                form.Controls.Add(new Button() { Content = "Продолжить" });
            }

            window.Content= form;
        }

        private void CreateButton(XmlStructureComplat.PayRecord payrec)
        {
            var frame = new StackPanel() { Orientation = Orientation.Vertical };
            var button = new Button();
            button.Content = payrec.Name;
            button.Command = model.NextPageCommand;
            button.CommandParameter = payrec;
            //button.SetBinding(Button.CommandProperty, "NextPageCommand");
            //button.SetBinding(Button.CommandParameterProperty, new Binding(){ Source = it });
            frame.Children.Add(button);
            frame.Children.Add(new TextBlock());
            form.Controls.Add(frame);
        }
    }
}
