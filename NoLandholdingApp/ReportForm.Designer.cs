namespace NoLandholdingApp
{
    partial class ReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.dataSet1 = new NoLandholdingApp.DataSet1();
            this.noLandholdingDataSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.noLandholding_DataSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.noLandholdingDataSourceBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLandholdingDataSourceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLandholding_DataSourceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLandholdingDataSourceBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // noLandholdingDataSourceBindingSource
            // 
            this.noLandholdingDataSourceBindingSource.DataMember = "noLandholding_DataSource";
            this.noLandholdingDataSourceBindingSource.DataSource = this.dataSet1;
            // 
            // noLandholding_DataSourceBindingSource
            // 
            this.noLandholding_DataSourceBindingSource.DataMember = "noLandholding_DataSource";
            this.noLandholding_DataSourceBindingSource.DataSource = this.dataSet1;
            // 
            // noLandholdingDataSourceBindingSource1
            // 
            this.noLandholdingDataSourceBindingSource1.DataMember = "noLandholding_DataSource";
            this.noLandholdingDataSourceBindingSource1.DataSource = this.dataSet1;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ReportForm";
            this.Text = "Print Preview";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLandholdingDataSourceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLandholding_DataSourceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noLandholdingDataSourceBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource noLandholdingDataSourceBindingSource;
        private DataSet1 dataSet1;
        private System.Windows.Forms.BindingSource noLandholding_DataSourceBindingSource;
        private System.Windows.Forms.BindingSource noLandholdingDataSourceBindingSource1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}