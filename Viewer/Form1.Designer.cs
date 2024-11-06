namespace Viewer
{
    sealed partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this._rightPanel = new System.Windows.Forms.Panel();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._leftPanel = new System.Windows.Forms.TableLayoutPanel();
            this._autoScrollPanel = new System.Windows.Forms.TableLayoutPanel();
            this._buttonPanel = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _rightPanel
            // 
            this._rightPanel.BackColor = System.Drawing.SystemColors.Control;
            this._rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightPanel.Location = new System.Drawing.Point(949, 3);
            this._rightPanel.Name = "_rightPanel";
            this._rightPanel.Size = new System.Drawing.Size(310, 634);
            this._rightPanel.TabIndex = 1;
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 3;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tableLayoutPanel.Controls.Add(this._rightPanel, 2, 0);
            this._tableLayoutPanel.Controls.Add(this._leftPanel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._autoScrollPanel, 1, 1);
            this._tableLayoutPanel.Controls.Add(this._buttonPanel, 2, 1);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 2;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(1262, 753);
            this._tableLayoutPanel.TabIndex = 0;
            // 
            // _leftPanel
            // 
            this._leftPanel.ColumnCount = 1;
            this._leftPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._leftPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._leftPanel.Location = new System.Drawing.Point(3, 3);
            this._leftPanel.Name = "_leftPanel";
            this._leftPanel.RowCount = 3;
            this._leftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._leftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._leftPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._leftPanel.Size = new System.Drawing.Size(309, 100);
            this._leftPanel.TabIndex = 4;
            // 
            // _autoScrollPanel
            // 
            this._autoScrollPanel.ColumnCount = 2;
            this._autoScrollPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._autoScrollPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._autoScrollPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._autoScrollPanel.Location = new System.Drawing.Point(318, 650);
            this._autoScrollPanel.Name = "_autoScrollPanel";
            this._autoScrollPanel.RowCount = 2;
            this._autoScrollPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._autoScrollPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._autoScrollPanel.Size = new System.Drawing.Size(625, 100);
            this._autoScrollPanel.TabIndex = 5;
            // 
            // _buttonPanel
            // 
            this._buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonPanel.ColumnCount = 2;
            this._buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._buttonPanel.Location = new System.Drawing.Point(1115, 686);
            this._buttonPanel.Name = "_buttonPanel";
            this._buttonPanel.RowCount = 1;
            this._buttonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._buttonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._buttonPanel.Size = new System.Drawing.Size(144, 64);
            this._buttonPanel.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 753);
            this.Controls.Add(this._tableLayoutPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this._tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel _rightPanel;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel _leftPanel;
        private System.Windows.Forms.TableLayoutPanel _autoScrollPanel;
        private System.Windows.Forms.TableLayoutPanel _buttonPanel;
    }
}

