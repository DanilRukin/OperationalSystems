using Messanger.Client.Infrastructure.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Client.Infrastructure
{
    public class Presenter
    {
        private IView _view;

        public Presenter(IView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void SetView(IView view)
        {
            _view = view;
        }

        public void Show() => _view.Show();
    }
}
