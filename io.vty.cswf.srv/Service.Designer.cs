namespace io.vty.cswf.srv
{
    partial class Service
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.evn_l = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.evn_l)).BeginInit();
            // 
            // Service
            // 
            this.ServiceName = "cswf.srv";
            ((System.ComponentModel.ISupportInitialize)(this.evn_l)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog evn_l;
    }
}
