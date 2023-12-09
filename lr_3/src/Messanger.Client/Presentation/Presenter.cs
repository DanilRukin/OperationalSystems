using Messanger.Client.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messanger.Client.Presentation
{
    public class Presenter
    {
        private IView _view;

        public Presenter()
        {
        }

        public void SetView(IView view)
        {
            _view = view;
        }

        public void Show()
        {
            bool show = true;
            while(show)
            {
                show = _view.Show();
            }
        }
    }
}
