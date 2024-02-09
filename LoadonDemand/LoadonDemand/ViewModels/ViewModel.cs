using LoadonDemand.Models;
using Syncfusion.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoadonDemand.ViewModels
{
    internal class ViewModel
    {
        public ObservableCollection<Model> Menu { get; set; }

        public ICommand? TreeViewOnDemandCommand
        {
            get; set;
        }

        public ViewModel()
        {
            this.Menu = this.GetMenuItems();
            TreeViewOnDemandCommand = new Command(ExecuteOnDemandLoading, CanExecuteOnDemandLoading);
        }

        private bool CanExecuteOnDemandLoading(object sender)
        {
            var hasChildNodes = ((sender as TreeViewNode)!.Content as Model)!.HasChildNodes;
            if (hasChildNodes)
                return true;
            else
                return false;
        }

        private void ExecuteOnDemandLoading(object obj)
        {
            var node = obj as TreeViewNode;
            // Skip the repeated population of child items when every time the node expands.
            if (node!.ChildNodes!.Count > 0)
            {
                node.IsExpanded = true;
                return;
            }

            node.ShowExpanderAnimation = true;
            //Animation starts for expander to show progressing of load on demand
            Model? Info = node!.Content as Model;
            Microsoft.Maui.Controls.Application.Current!.Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(1000);
                //Fetching child items to add
                var items = GetSubMenu(Info!.ID);
                // Populating child items for the node in on-demand
                node.PopulateChildNodes(items);
                if (items.Any())
                    //Expand the node after child items are added.
                    node.IsExpanded = true;
                node.ShowExpanderAnimation = false;
            });
        }

        private ObservableCollection<Model> GetMenuItems()
        {
            ObservableCollection<Model> menuItems = new ObservableCollection<Model>();
            menuItems.Add(new Model() { ItemName = "My Drive", HasChildNodes = true, ID = 0 });
            return menuItems;
        }

        public IEnumerable<Model> GetSubMenu(int iD)
        {
            ObservableCollection<Model> menuItems = new ObservableCollection<Model>();
            if (iD == 0)
            {
                menuItems.Add(new Model() { ItemName = "Documents", HasChildNodes = true, ID = 1 });
                menuItems.Add(new Model() { ItemName = "Work", HasChildNodes = true, ID = 2 });
                menuItems.Add(new Model() { ItemName = "Photos", HasChildNodes = true, ID = 3 });
                menuItems.Add(new Model() { ItemName = "Music", HasChildNodes = true, ID = 4 });
                menuItems.Add(new Model() { ItemName = "Videos", HasChildNodes = true, ID = 5 });
            }
            if (iD == 1)
            {
                menuItems.Add(new Model() { ItemName = "Personal", HasChildNodes = true, ID = 11 });
            }
            else if (iD == 2)
            {
                menuItems.Add(new Model() { ItemName = "ProjectA", HasChildNodes = true, ID = 21 });
                menuItems.Add(new Model() { ItemName = "ProjectB", HasChildNodes = true, ID = 22 });
            }
            else if (iD == 3)
            {
                menuItems.Add(new Model() { ItemName = "Family", HasChildNodes = true, ID = 31 });
                menuItems.Add(new Model() { ItemName = "Friends", HasChildNodes = true, ID = 32 });
            }
            else if (iD == 4)
            {
                menuItems.Add(new Model() { ItemName = "Playlist1", HasChildNodes = true, ID = 41 });
                menuItems.Add(new Model() { ItemName = "Playlist2", HasChildNodes = true, ID = 42 });
            }
            else if (iD == 5)
            {
                menuItems.Add(new Model() { ItemName = "Tutorial", HasChildNodes = true, ID = 51 });
            }
            else if (iD == 11)
            {
                menuItems.Add(new Model() { ItemName = "Resume.docx", HasChildNodes = false, ID = 61 });
                menuItems.Add(new Model() { ItemName = "TravelPlans.pdf", HasChildNodes = false, ID = 62 });
            }
            else if (iD == 21)
            {
                menuItems.Add(new Model() { ItemName = "Proposal.docx", HasChildNodes = false, ID = 71 });
                menuItems.Add(new Model() { ItemName = "Presentation.pptx", HasChildNodes = false, ID = 72 });
            }
            else if (iD == 22)
            {
                menuItems.Add(new Model() { ItemName = "Report.pdf", HasChildNodes = false, ID = 73 });
            }
            else if (iD == 31)
            {
                menuItems.Add(new Model() { ItemName = "Beach.jpg", HasChildNodes = false, ID = 81 });
                menuItems.Add(new Model() { ItemName = "Mountains.jpg", HasChildNodes = false, ID = 82 });
            }
            else if (iD == 32)
            {
                menuItems.Add(new Model() { ItemName = "GrouPhoto.jpg", HasChildNodes = false, ID = 91 });
            }
            else if (iD == 41)
            {
                menuItems.Add(new Model() { ItemName = "Song1.mp3", HasChildNodes = false, ID = 101 });
                menuItems.Add(new Model() { ItemName = "Song2.mp3", HasChildNodes = false, ID = 102 });
            }
            else if (iD == 42)
            {
                menuItems.Add(new Model() { ItemName = "Song3.mp3", HasChildNodes = false, ID = 111 });
                menuItems.Add(new Model() { ItemName = "Song4.mp3", HasChildNodes = false, ID = 112 });
            }
            else if (iD == 51)
            {
                menuItems.Add(new Model() { ItemName = "Codingbasics.mp4", HasChildNodes = false, ID = 121 });
                menuItems.Add(new Model() { ItemName = "Webdevelopment.mp4", HasChildNodes = false, ID = 122 });
            }

            return menuItems;
        }
    }
}
