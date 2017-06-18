using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NumbrTumblr
{
    public partial class NumberSetNumberTabbedPage : TabbedPage
    {
        private NumberSetPage _numberSetPage = null;
        private NumberSetNumbersPage _numberSetNumbersPage = null;

        public NumberSetPage NumberSetPage
        {
            get{ return _numberSetPage; }
            set { _numberSetPage = value; }
        }

        public NumberSetNumbersPage NumberSetNumbersPage
        {
            get { return _numberSetNumbersPage; }
            set { _numberSetNumbersPage = value; }
        }
        public NumberSetNumberTabbedPage(NumberSetPage numberSetPage, NumberSetNumbersPage numberSetNumbersPage)
        {
            InitializeComponent();
            _numberSetPage = numberSetPage;
            _numberSetNumbersPage = numberSetNumbersPage;
            GenerateContent();           
        }

        public void GenerateContent()
        {
            Children.Add(_numberSetPage);
            Children.Add(_numberSetNumbersPage);
        }
    }
}
