# perform-load-on-demand-item-population-in-.net-maui-treeview
This repository demonstrates how to implement load-on-demand item population in the .NET MAUI TreeView (SfTreeView) control. It provides a sample implementation using the LoadOnDemandCommand to dynamically load child nodes only when a parent node is expanded, improving performance and responsiveness for large hierarchical data sets.

## Sample

### XAML

```xaml
<ContentPage.BindingContext>
    <local:ViewModel x:Name="viewModel"/>
</ContentPage.BindingContext>
<ContentPage.Content>
    <sfTreeView:SfTreeView x:Name="treeView"
                           LoadOnDemandCommand="{Binding TreeViewOnDemandCommand}"
                           ItemsSource="{Binding Menu}"
                           ExpandActionTarget="Node"
                           FullRowSelect="True">
        
    </sfTreeView:SfTreeView>
</ContentPage.Content>
```

### View Model

```csharp
internal class ViewModel
{

    public ICommand? TreeViewOnDemandCommand
    {
        get; set;
    }

    public ViewModel()
    {
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
}
```

## Requirements to run the demo

To run the demo, refer to [System Requirements for .NET MAUI](https://help.syncfusion.com/maui/system-requirements).

Make sure that you have the compatible versions of [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/ ) with the Dot NET MAUI workload and [.NET Core SDK 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) or later version in your machine before starting to work on this project.

## Troubleshooting:
### Path too long exception

If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.

## License

Syncfusion® has no liability for any damage or consequence that may arise from using or viewing the samples. The samples are for demonstrative purposes. If you choose to use or access the samples, you agree to not hold Syncfusion® liable, in any form, for any damage related to use, for accessing, or viewing the samples. By accessing, viewing, or seeing the samples, you acknowledge and agree Syncfusion®'s samples will not allow you seek injunctive relief in any form for any claim related to the sample. If you do not agree to this, do not view, access, utilize, or otherwise do anything with Syncfusion®'s samples.