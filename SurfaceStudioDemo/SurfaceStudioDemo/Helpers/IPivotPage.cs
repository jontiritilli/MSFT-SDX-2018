using System.Threading.Tasks;

namespace SurfaceStudioDemo.Helpers
{
    public interface IPivotPage
    {
        Task OnPivotSelectedAsync();

        Task OnPivotUnselectedAsync();
    }
}
