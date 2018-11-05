using System.Threading.Tasks;

namespace SurfaceHeadphoneDemo.Helpers
{
    public interface IPivotPage
    {
        Task OnPivotSelectedAsync();

        Task OnPivotUnselectedAsync();
    }
}
