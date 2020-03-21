namespace SpotifyRecommendationEngine
{
    partial class MainForm
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
            this.dgvSeeds = new System.Windows.Forms.DataGridView();
            this.dgvRecos = new System.Windows.Forms.DataGridView();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvTuneable = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeeds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTuneable)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSeeds
            // 
            this.dgvSeeds.AllowUserToOrderColumns = true;
            this.dgvSeeds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSeeds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSeeds.Location = new System.Drawing.Point(0, 2);
            this.dgvSeeds.Name = "dgvSeeds";
            this.dgvSeeds.Size = new System.Drawing.Size(897, 135);
            this.dgvSeeds.TabIndex = 0;
            // 
            // dgvRecos
            // 
            this.dgvRecos.AllowUserToOrderColumns = true;
            this.dgvRecos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRecos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecos.Location = new System.Drawing.Point(0, 217);
            this.dgvRecos.Name = "dgvRecos";
            this.dgvRecos.Size = new System.Drawing.Size(903, 358);
            this.dgvRecos.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(748, 143);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(149, 68);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvTuneables
            // 
            this.dgvTuneable.AllowUserToOrderColumns = true;
            this.dgvTuneable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTuneable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTuneable.Location = new System.Drawing.Point(0, 143);
            this.dgvTuneable.Name = "dgvTuneables";
            this.dgvTuneable.Size = new System.Drawing.Size(742, 68);
            this.dgvTuneable.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 574);
            this.Controls.Add(this.dgvSeeds);
            this.Controls.Add(this.dgvTuneable);
            this.Controls.Add(this.dgvRecos);
            this.Controls.Add(this.btnSearch);
            this.Name = "MainForm";
            this.Text = "Spotify Recommendation Engine";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeeds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTuneable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSeeds;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvRecos;
        private System.Windows.Forms.DataGridView dgvTuneable;
    }
}

