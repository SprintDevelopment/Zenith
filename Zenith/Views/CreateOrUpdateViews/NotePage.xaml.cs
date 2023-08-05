using Zenith.Models;
using Zenith.Repositories;

namespace Zenith.Views.CreateOrUpdateViews
{
    /// <summary>
    /// Interaction logic for NotePage.xaml
    /// </summary>
    public partial class NotePage : BaseCreateOrUpdatePage<Note>
    {
        public NotePage()
        {
            InitializeComponent();

            Initialize(new NoteRepository());
        }
    }
}
