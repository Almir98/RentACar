﻿using RentaCar.Data.Requests.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar.WinUI.Forms
{
    public partial class frmCustomerDetails : Form
    {
        private readonly APIService _apiservice=new APIService("Customer");
        private int? _id = null;

        public frmCustomerDetails(int? id)
        {
            InitializeComponent();
            _id = id;
        }

        private async void frmCustomerDetails_Load(object sender, EventArgs e)
        {
            if(_id.HasValue)
            {
                var customer = await _apiservice.GetById<CustomerRequest>(_id);

                txtFirstName.Text = customer.FirstName;
                txtLastName.Text = customer.LastName;
                txtPhone.Text = customer.Phone;
                txtEmail.Text = customer.Email;
                txtCityName.Text = customer.CityName;
            }
        }
    }
}
