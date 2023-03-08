using NetEti.Globals;

namespace NetEti.DemoApplications
{
    /// <summary>
    /// <para xml:lang="de">
    /// Haupt-Form.
    /// </para>
    /// <para xml:lang="en">
    /// Main-Form.
    /// </para>
    /// </summary>
    public partial class Form1 : Form
    {
        private bool _lockOk;

        /// <summary>
        /// <para xml:lang="de">
        /// Demo
        /// </para>
        /// <para xml:lang="en">
        /// Demo
        /// </para>
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BeispielClass MeinBeispiel = GenericSingletonProvider.GetInstance<BeispielClass>();
            BeispielClass MeinBeispiel2 = GenericSingletonProvider.GetInstance<BeispielClass>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThreadLocker.LockNameGlobal("Lock1");
            this.label1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ThreadLocker.UnlockNameGlobal("Lock1");
            this.label1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.label1.Text = "";
            Task.Factory.StartNew(() => { ; });
            Task task = new Task(new Action(TryLockit));
            task.Start();
            task.Wait();
            this.label1.Text = this._lockOk ? "erfolgreich" : "fehlgeschlagen";
        }

        private void TryLockit()
        {
            this._lockOk = ThreadLocker.TryLockNameGlobal("Lock1", 5000);
        }

    }
}
