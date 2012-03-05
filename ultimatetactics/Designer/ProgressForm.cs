using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace UltimateTactics.Designer
{
  /// <summary>
  /// Summary description for ProgressWindow.
  /// </summary>
  public class ProgressWindow : Form, IProgressCallback
  {
    private Button mCancelButton;
    private Label mLabel;
    private ProgressBar mProgressBar;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private Container mComponents = null;

    public delegate void SetTextInvoker(String text);
    public delegate void IncrementInvoker(int val);
    public delegate void StepToInvoker(int val);
    public delegate void RangeInvoker(int minimum, int maximum);

    private String mTitleRoot = "";
    private ManualResetEvent mInitEvent = new ManualResetEvent(false);
    private ManualResetEvent mAbortEvent = new ManualResetEvent(false);
    private bool mRequiresClose = true;

    public ProgressWindow()
    {
      InitializeComponent();
    }

    #region Implementation of IProgressCallback
    /// <summary>
    /// Call this method from the worker thread to initialize
    /// the progress meter.
    /// </summary>
    /// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
    /// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
    public void Begin(int minimum, int maximum)
    {
      mInitEvent.WaitOne();
      Invoke(new RangeInvoker(DoBegin), new object[] { minimum, maximum });
    }

    /// <summary>
    /// Call this method from the worker thread to initialize
    /// the progress callback, without setting the range
    /// </summary>
    public void Begin()
    {
      mInitEvent.WaitOne();
      Invoke(new MethodInvoker(DoBegin));
    }

    /// <summary>
    /// Call this method from the worker thread to reset the range in the progress callback
    /// </summary>
    /// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
    /// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
    /// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
    public void SetRange(int minimum, int maximum)
    {
      mInitEvent.WaitOne();
      Invoke(new RangeInvoker(DoSetRange), new object[] { minimum, maximum });
    }

    /// <summary>
    /// Call this method from the worker thread to update the progress text.
    /// </summary>
    /// <param name="text">The progress text to display</param>
    public void SetText(String text)
    {
      Invoke(new SetTextInvoker(DoSetText), new object[] { text });
    }

    /// <summary>
    /// Call this method from the worker thread to increase the progress counter by a specified value.
    /// </summary>
    /// <param name="val">The amount by which to increment the progress indicator</param>
    public void Increment(int val)
    {
      Invoke(new IncrementInvoker(DoIncrement), new object[] { val });
    }

    /// <summary>
    /// Call this method from the worker thread to step the progress meter to a particular value.
    /// </summary>
    /// <param name="val"></param>
    public void StepTo(int val)
    {
      Invoke(new StepToInvoker(DoStepTo), new object[] { val });
    }


    /// <summary>
    /// If this property is true, then you should abort work
    /// </summary>
    public bool IsAborting
    {
      get
      {
        return mAbortEvent.WaitOne(0, false);
      }
    }

    /// <summary>
    /// Call this method from the worker thread to finalize the progress meter
    /// </summary>
    public void End()
    {
      if (mRequiresClose)
      {
        Invoke(new MethodInvoker(DoEnd));
      }
    }
    #endregion

    #region Implementation members invoked on the owner thread
    private void DoSetText(String text)
    {
      mLabel.Text = text;
    }

    private void DoIncrement(int val)
    {
      mProgressBar.Increment(val);
      UpdateStatusText();
    }

    private void DoStepTo(int val)
    {
      mProgressBar.Value = val;
      UpdateStatusText();
    }

    private void DoBegin(int minimum, int maximum)
    {
      DoBegin();
      DoSetRange(minimum, maximum);
    }

    private void DoBegin()
    {
      mCancelButton.Enabled = true;
      ControlBox = true;
    }

    private void DoSetRange(int minimum, int maximum)
    {
      mProgressBar.Minimum = minimum;
      mProgressBar.Maximum = maximum;
      mProgressBar.Value = minimum;
      mTitleRoot = Text;
    }

    private void DoEnd()
    {
      Close();
    }
    #endregion

    #region Overrides
    /// <summary>
    /// Handles the form load, and sets an event to ensure that
    /// intialization is synchronized with the appearance of the form.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(System.EventArgs e)
    {
      base.OnLoad(e);
      ControlBox = false;
      mInitEvent.Set();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (mComponents != null)
        {
          mComponents.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    /// <summary>
    /// Handler for 'Close' clicking
    /// </summary>
    /// <param name="e"></param>
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      mRequiresClose = false;
      AbortWork();
      base.OnClosing(e);
    }
    #endregion

    #region Implementation Utilities
    /// <summary>
    /// Utility function that formats and updates the title bar text
    /// </summary>
    private void UpdateStatusText()
    {
      Text = mTitleRoot + String.Format(" - {0}% complete", (mProgressBar.Value * 100) / (mProgressBar.Maximum - mProgressBar.Minimum));
    }

    /// <summary>
    /// Utility function to terminate the thread
    /// </summary>
    private void AbortWork()
    {
      mAbortEvent.Set();
    }
    #endregion

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.mProgressBar = new System.Windows.Forms.ProgressBar();
      this.mLabel = new System.Windows.Forms.Label();
      this.mCancelButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // progressBar
      // 
      this.mProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right);
      this.mProgressBar.Location = new System.Drawing.Point(8, 80);
      this.mProgressBar.Name = "progressBar";
      this.mProgressBar.Size = new System.Drawing.Size(192, 23);
      this.mProgressBar.TabIndex = 1;
      // 
      // label
      // 
      this.mLabel.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right);
      this.mLabel.Location = new System.Drawing.Point(8, 8);
      this.mLabel.Name = "label";
      this.mLabel.Size = new System.Drawing.Size(272, 64);
      this.mLabel.TabIndex = 0;
      this.mLabel.Text = "Converting to video...";
      // 
      // cancelButton
      // 
      this.mCancelButton.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
      this.mCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.mCancelButton.Enabled = false;
      this.mCancelButton.Location = new System.Drawing.Point(208, 80);
      this.mCancelButton.Name = "cancelButton";
      this.mCancelButton.TabIndex = 2;
      this.mCancelButton.Text = "Cancel";
      // 
      // ProgressWindow
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(290, 114);
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                      this.mCancelButton,
                                      this.mProgressBar,
                                      this.mLabel});
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = "ProgressWindow";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Converting to video...";
      this.ResumeLayout(false);

    }
    #endregion


  }
}
