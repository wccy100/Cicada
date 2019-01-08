﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using IoT.Sharp.Sdk.CSharp;

namespace IoT.Sharp.WinForm
{
    public partial class frmInstaller : DevExpress.XtraEditors.XtraForm
    {
        public frmInstaller()
        {
            InitializeComponent();
        }

        private void btnInstall_ClickAsync(object sender, EventArgs e)
        {
            var dot = installDtoBindingSource.Current as InstallDto;
            if (dot != null)
            {
                btnInstall.Enabled = false;
                Task.Run(async () =>
               {
                   try
                   {
                       var Client = SdkClient.Create<InstallerClient>();
                       var result = await Client.InstallAsync(dot);
                       Invoke((MethodInvoker)  delegate
                       {
                           btnInstall.Enabled = true;
                           if (result.Installed)
                           {
                               DialogResult = DialogResult.OK;
                           }
                       });
                   }
                   catch (SwaggerException se)
                   {
                       this.Invoke((MethodInvoker)delegate
                         {
                             libinfo.Text = se.ToResult().msg;
                             btnInstall.Enabled = true;
                         });
                   }
               });
            }
        }

        private void frmInstaller_Load(object sender, EventArgs e)
        {
            installDtoBindingSource.DataSource = new InstallDto()
            {
                CustomerEMail = "iotmaster@iotsharp.net",
                CustomerName = "iotsharp",
                TenantName = "iotsharp",
                TenantEMail = "iotmaster@iotsharp.net",
                PhoneNumber = "400000000",
                Email = "iotmaster@iotsharp.net"
            };
        }
    }
}