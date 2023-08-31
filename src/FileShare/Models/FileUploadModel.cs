using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FileShare.Models;

public class FileUploadModel : IEquatable<FileUploadModel>
{
    public IBrowserFile File { get; init; }
    public RenderFragment RenderFragment { get; set; }

    public FileUploadModel(IBrowserFile file)
    {
        File = file;
        RenderFragment = builder => { };
    }

    public bool Equals(FileUploadModel? other)
    {
        if (other is null)
        {
            return false;
        }

        return File.Name.Equals(other.File.Name);
    }

    public void SetState(IUploadingState state)
    {
        RenderFragment = state.StatusFragment();
    }

    public interface IUploadingState
    {
        RenderFragment StatusFragment();
    }

    public class WaitingState : IUploadingState
    {
        public RenderFragment StatusFragment()
        {
            return builder =>
            {
                builder.OpenComponent<MudIcon>(0);
                builder.AddAttribute(1, "Icon", Icons.Material.Rounded.AccessTime);
                builder.CloseComponent();
            };
        }
    }

    public class UploadedState : IUploadingState
    {
        public RenderFragment StatusFragment()
        {
            return builder =>
            {
                builder.OpenComponent<MudIcon>(0);
                builder.AddAttribute(1, "Icon", Icons.Material.Rounded.Done);
                builder.AddAttribute(2, "Color", Color.Success);
                builder.CloseComponent();
            };
        }
    }

    public class ErrorState : IUploadingState
    {
        public RenderFragment StatusFragment()
        {
            return builder =>
            {
                builder.OpenComponent<MudIcon>(0);
                builder.AddAttribute(1, "Icon", Icons.Material.Rounded.Warning);
                builder.AddAttribute(2, "Color", Color.Warning);
                builder.CloseComponent();
            };
        }
    }

    public class PercentageState : IUploadingState
    {
        public int Value { get; set; }
        public RenderFragment StatusFragment()
        {
            return builder =>
            {
                builder.OpenComponent<MudText>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)((builder2) =>
                {
                    builder2.AddContent(2, $"{Value}%");
                }));
                builder.CloseComponent();
            };
        }
    }
}
