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
        #region fields
        private DynamicMenuWindowViewModel model;
        private Window window;
        private FormAround form = new FormAround();
        private PagesList pages = new PagesList();
        #endregion
        #region ctor
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
        #endregion

        private void IsLoadingMenuChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsLoadingMenu")
            {
                window.Content = null;
                window.Content = "Loading...";
            }
        }

        #region Properties
        public bool IsPageAvaiable => pages.IsNextAvaible;
        #endregion
        private void BuildMenuOnReponse()
        {
            if (model == null) return;
            var rootResponse = model.Responce;
            var resp = rootResponse.GetListResponse;
            var paylist = resp.PayRecord;
            if (paylist.Count > 1)
            {
                pages.New();
                foreach (var payrec in paylist)
                {
                    var button = ButtonSelect(payrec);
                    pages.AddControl(button);
                }
            }
            if (paylist.Count == 1)
            {
                var attrRecords = paylist.First().AttrRecord;
                foreach (var attr in attrRecords)
                {
                    //Display every lookup as 1 page
                    if (attr.Edit == 1 && attr.View == 1 && !string.IsNullOrEmpty(attr.Lookup))
                    {
                        var lookups = paylist.First().Lookups;
                        var selectedLookup = lookups.Where(x => x.Name.ToLower() == attr.Lookup.ToLower()).Single();
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
                            form.Controls.Add(new TextBlock());
                        });
                        new Window() { Content = form, Width = 450, Height = 800 }.Show();
                        form = new FormAround();
                    }
                }

                foreach (var it in attrRecords)
                {
                    //first display filled data attrs
                    if (it.Edit != 1 && it.View == 1)
                    {
                        var label = new Label();
                        label.Content = $"{it.Name} = {it.Value}";

                        form.Controls.Add(label);
                        form.Controls.Add(new TextBlock());
                    }
                }
                foreach (var it in attrRecords)
                {
                    //at last display attrs need to enter with textbox
                    if (it.Edit == 1 && it.View == 1 && string.IsNullOrEmpty(it.Lookup))
                    {
                        var label = new Label();
                        label.Content = $"{it.Name}:";
                        var inputbox = new TextBox();

                        form.Controls.Add(label);
                        form.Controls.Add(inputbox);
                        form.Controls.Add(new TextBlock());
                    }
                }

                form.Controls.Add(new TextBlock());
                form.Controls.Add(new TextBlock());
                form.Controls.Add(new Button() { Content = "Продолжить" });
            }

            window.Content= form;
        }

        private StackPanel ButtonSelect(XmlStructureComplat.PayRecord payrec)
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
            //form.Controls.Add(frame);
            return frame;
        }
    }
}
