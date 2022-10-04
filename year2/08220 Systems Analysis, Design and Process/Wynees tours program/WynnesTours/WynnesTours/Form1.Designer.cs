namespace WynnesTours
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
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxTours = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCreateTour = new System.Windows.Forms.Button();
            this.buttonEditTour = new System.Windows.Forms.Button();
            this.buttonDeleteTour = new System.Windows.Forms.Button();
            this.textBoxTourName = new System.Windows.Forms.TextBox();
            this.textBoxCoachID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxBaseTicketPrice = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TourIDTB = new System.Windows.Forms.TextBox();
            this.dateTimePickerTourDate = new System.Windows.Forms.DateTimePicker();
            this.richTextBoxTourDescription = new System.Windows.Forms.RichTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCustomer = new System.Windows.Forms.TabPage();
            this.buttonCustomerSearch = new System.Windows.Forms.Button();
            this.textBoxCustomerSearch = new System.Windows.Forms.TextBox();
            this.checkBoxGoldClub = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.buttonCreateCustomer = new System.Windows.Forms.Button();
            this.buttonEditCustomer = new System.Windows.Forms.Button();
            this.buttonDeleteCustomer = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxTelephoneNo = new System.Windows.Forms.TextBox();
            this.textBoxForename = new System.Windows.Forms.TextBox();
            this.textBoxSurname = new System.Windows.Forms.TextBox();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.listBoxCustomers = new System.Windows.Forms.ListBox();
            this.tabPageCoaches = new System.Windows.Forms.TabPage();
            this.label27 = new System.Windows.Forms.Label();
            this.textBoxPassMani = new System.Windows.Forms.TextBox();
            this.buttonPassMani = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.CoachIDTB = new System.Windows.Forms.TextBox();
            this.buttonDeleteCoach = new System.Windows.Forms.Button();
            this.buttonAddCoach = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.listBoxCoaches = new System.Windows.Forms.ListBox();
            this.tabPageTickets = new System.Windows.Forms.TabPage();
            this.buttonFinReport = new System.Windows.Forms.Button();
            this.BuyTicketTourID = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.TicketTourID = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.buttonTicketSearchCustomerID = new System.Windows.Forms.Button();
            this.buttonTicketSearchTicketID = new System.Windows.Forms.Button();
            this.buttonTicketSearchTourID = new System.Windows.Forms.Button();
            this.textBoxTicketSearch = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.TicketReservationIDTB = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.dateTimePickerReservation = new System.Windows.Forms.DateTimePicker();
            this.label22 = new System.Windows.Forms.Label();
            this.reserveCustomerIDTB = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.buttonClearReservation = new System.Windows.Forms.Button();
            this.buttonReserveTicket = new System.Windows.Forms.Button();
            this.listBoxTickets = new System.Windows.Forms.ListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.bookCustomerIDTB = new System.Windows.Forms.TextBox();
            this.PricePaidTB = new System.Windows.Forms.TextBox();
            this.buttonBookTicket = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageCustomer.SuspendLayout();
            this.tabPageCoaches.SuspendLayout();
            this.tabPageTickets.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F);
            this.label1.Location = new System.Drawing.Point(432, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 54);
            this.label1.TabIndex = 0;
            this.label1.Text = "Wynne\'s Tours";
            // 
            // listBoxTours
            // 
            this.listBoxTours.FormattingEnabled = true;
            this.listBoxTours.Location = new System.Drawing.Point(11, 83);
            this.listBoxTours.Name = "listBoxTours";
            this.listBoxTours.Size = new System.Drawing.Size(406, 290);
            this.listBoxTours.TabIndex = 1;
            this.listBoxTours.DoubleClick += new System.EventHandler(this.listBoxTours_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tours";
            // 
            // buttonCreateTour
            // 
            this.buttonCreateTour.Location = new System.Drawing.Point(171, 577);
            this.buttonCreateTour.Name = "buttonCreateTour";
            this.buttonCreateTour.Size = new System.Drawing.Size(75, 23);
            this.buttonCreateTour.TabIndex = 5;
            this.buttonCreateTour.Text = "Create Tour";
            this.buttonCreateTour.UseVisualStyleBackColor = true;
            this.buttonCreateTour.Click += new System.EventHandler(this.buttonCreateTour_Click);
            // 
            // buttonEditTour
            // 
            this.buttonEditTour.Location = new System.Drawing.Point(252, 577);
            this.buttonEditTour.Name = "buttonEditTour";
            this.buttonEditTour.Size = new System.Drawing.Size(75, 23);
            this.buttonEditTour.TabIndex = 6;
            this.buttonEditTour.Text = "Edit Tour";
            this.buttonEditTour.UseVisualStyleBackColor = true;
            this.buttonEditTour.Click += new System.EventHandler(this.buttonEditTour_Click);
            // 
            // buttonDeleteTour
            // 
            this.buttonDeleteTour.Location = new System.Drawing.Point(333, 577);
            this.buttonDeleteTour.Name = "buttonDeleteTour";
            this.buttonDeleteTour.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteTour.TabIndex = 7;
            this.buttonDeleteTour.Text = "Delete tour";
            this.buttonDeleteTour.UseVisualStyleBackColor = true;
            this.buttonDeleteTour.Click += new System.EventHandler(this.buttonDeleteTour_Click);
            // 
            // textBoxTourName
            // 
            this.textBoxTourName.Location = new System.Drawing.Point(110, 381);
            this.textBoxTourName.Name = "textBoxTourName";
            this.textBoxTourName.Size = new System.Drawing.Size(307, 20);
            this.textBoxTourName.TabIndex = 12;
            // 
            // textBoxCoachID
            // 
            this.textBoxCoachID.Location = new System.Drawing.Point(110, 497);
            this.textBoxCoachID.Name = "textBoxCoachID";
            this.textBoxCoachID.Size = new System.Drawing.Size(307, 20);
            this.textBoxCoachID.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 384);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Tour Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 407);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Description";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 500);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Coach ID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(49, 477);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Tour Date";
            // 
            // textBoxBaseTicketPrice
            // 
            this.textBoxBaseTicketPrice.Location = new System.Drawing.Point(110, 523);
            this.textBoxBaseTicketPrice.Name = "textBoxBaseTicketPrice";
            this.textBoxBaseTicketPrice.Size = new System.Drawing.Size(307, 20);
            this.textBoxBaseTicketPrice.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 526);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Base Ticket Price";
            // 
            // TourIDTB
            // 
            this.TourIDTB.Location = new System.Drawing.Point(110, 549);
            this.TourIDTB.Name = "TourIDTB";
            this.TourIDTB.Size = new System.Drawing.Size(307, 20);
            this.TourIDTB.TabIndex = 21;
            // 
            // dateTimePickerTourDate
            // 
            this.dateTimePickerTourDate.Location = new System.Drawing.Point(110, 471);
            this.dateTimePickerTourDate.Name = "dateTimePickerTourDate";
            this.dateTimePickerTourDate.Size = new System.Drawing.Size(270, 20);
            this.dateTimePickerTourDate.TabIndex = 22;
            // 
            // richTextBoxTourDescription
            // 
            this.richTextBoxTourDescription.Location = new System.Drawing.Point(110, 407);
            this.richTextBoxTourDescription.Name = "richTextBoxTourDescription";
            this.richTextBoxTourDescription.Size = new System.Drawing.Size(307, 58);
            this.richTextBoxTourDescription.TabIndex = 23;
            this.richTextBoxTourDescription.Text = "";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageCustomer);
            this.tabControl.Controls.Add(this.tabPageCoaches);
            this.tabControl.Controls.Add(this.tabPageTickets);
            this.tabControl.Location = new System.Drawing.Point(423, 66);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(827, 531);
            this.tabControl.TabIndex = 24;
            // 
            // tabPageCustomer
            // 
            this.tabPageCustomer.Controls.Add(this.buttonCustomerSearch);
            this.tabPageCustomer.Controls.Add(this.textBoxCustomerSearch);
            this.tabPageCustomer.Controls.Add(this.checkBoxGoldClub);
            this.tabPageCustomer.Controls.Add(this.label3);
            this.tabPageCustomer.Controls.Add(this.textBoxTitle);
            this.tabPageCustomer.Controls.Add(this.buttonCreateCustomer);
            this.tabPageCustomer.Controls.Add(this.buttonEditCustomer);
            this.tabPageCustomer.Controls.Add(this.buttonDeleteCustomer);
            this.tabPageCustomer.Controls.Add(this.label17);
            this.tabPageCustomer.Controls.Add(this.label16);
            this.tabPageCustomer.Controls.Add(this.label15);
            this.tabPageCustomer.Controls.Add(this.label14);
            this.tabPageCustomer.Controls.Add(this.label13);
            this.tabPageCustomer.Controls.Add(this.label12);
            this.tabPageCustomer.Controls.Add(this.textBoxTelephoneNo);
            this.tabPageCustomer.Controls.Add(this.textBoxForename);
            this.tabPageCustomer.Controls.Add(this.textBoxSurname);
            this.tabPageCustomer.Controls.Add(this.textBoxAddress);
            this.tabPageCustomer.Controls.Add(this.textBoxEmail);
            this.tabPageCustomer.Controls.Add(this.listBoxCustomers);
            this.tabPageCustomer.Location = new System.Drawing.Point(4, 22);
            this.tabPageCustomer.Name = "tabPageCustomer";
            this.tabPageCustomer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCustomer.Size = new System.Drawing.Size(819, 505);
            this.tabPageCustomer.TabIndex = 0;
            this.tabPageCustomer.Text = "Customers";
            this.tabPageCustomer.UseVisualStyleBackColor = true;
            // 
            // buttonCustomerSearch
            // 
            this.buttonCustomerSearch.Location = new System.Drawing.Point(731, 23);
            this.buttonCustomerSearch.Name = "buttonCustomerSearch";
            this.buttonCustomerSearch.Size = new System.Drawing.Size(78, 20);
            this.buttonCustomerSearch.TabIndex = 68;
            this.buttonCustomerSearch.Text = "Search";
            this.buttonCustomerSearch.UseVisualStyleBackColor = true;
            this.buttonCustomerSearch.Click += new System.EventHandler(this.CustomerSearch);
            // 
            // textBoxCustomerSearch
            // 
            this.textBoxCustomerSearch.Location = new System.Drawing.Point(418, 23);
            this.textBoxCustomerSearch.Name = "textBoxCustomerSearch";
            this.textBoxCustomerSearch.Size = new System.Drawing.Size(307, 20);
            this.textBoxCustomerSearch.TabIndex = 67;
            this.textBoxCustomerSearch.Text = "Search by ID";
            // 
            // checkBoxGoldClub
            // 
            this.checkBoxGoldClub.AutoSize = true;
            this.checkBoxGoldClub.Location = new System.Drawing.Point(502, 229);
            this.checkBoxGoldClub.Name = "checkBoxGoldClub";
            this.checkBoxGoldClub.Size = new System.Drawing.Size(15, 14);
            this.checkBoxGoldClub.TabIndex = 66;
            this.checkBoxGoldClub.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(444, 229);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 65;
            this.label3.Text = "Gold Club";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(503, 68);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(307, 20);
            this.textBoxTitle.TabIndex = 64;
            // 
            // buttonCreateCustomer
            // 
            this.buttonCreateCustomer.Location = new System.Drawing.Point(516, 249);
            this.buttonCreateCustomer.Name = "buttonCreateCustomer";
            this.buttonCreateCustomer.Size = new System.Drawing.Size(97, 23);
            this.buttonCreateCustomer.TabIndex = 63;
            this.buttonCreateCustomer.Text = "Create Customer";
            this.buttonCreateCustomer.UseVisualStyleBackColor = true;
            this.buttonCreateCustomer.Click += new System.EventHandler(this.buttonCreateCustomer_Click);
            // 
            // buttonEditCustomer
            // 
            this.buttonEditCustomer.Location = new System.Drawing.Point(619, 249);
            this.buttonEditCustomer.Name = "buttonEditCustomer";
            this.buttonEditCustomer.Size = new System.Drawing.Size(89, 23);
            this.buttonEditCustomer.TabIndex = 62;
            this.buttonEditCustomer.Text = "Edit Customer";
            this.buttonEditCustomer.UseVisualStyleBackColor = true;
            this.buttonEditCustomer.Click += new System.EventHandler(this.buttonEditCustomer_Click);
            // 
            // buttonDeleteCustomer
            // 
            this.buttonDeleteCustomer.Location = new System.Drawing.Point(714, 249);
            this.buttonDeleteCustomer.Name = "buttonDeleteCustomer";
            this.buttonDeleteCustomer.Size = new System.Drawing.Size(93, 23);
            this.buttonDeleteCustomer.TabIndex = 61;
            this.buttonDeleteCustomer.Text = "Delete Customer";
            this.buttonDeleteCustomer.UseVisualStyleBackColor = true;
            this.buttonDeleteCustomer.Click += new System.EventHandler(this.buttonDeleteCustomer_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(422, 201);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(75, 13);
            this.label17.TabIndex = 60;
            this.label17.Text = "Telephone No";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(464, 175);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 13);
            this.label16.TabIndex = 59;
            this.label16.Text = "Email";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(451, 149);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 13);
            this.label15.TabIndex = 58;
            this.label15.Text = "Address";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(447, 123);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 13);
            this.label14.TabIndex = 57;
            this.label14.Text = "Surname";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(443, 97);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 13);
            this.label13.TabIndex = 56;
            this.label13.Text = "Forename";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(470, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(27, 13);
            this.label12.TabIndex = 55;
            this.label12.Text = "Title";
            // 
            // textBoxTelephoneNo
            // 
            this.textBoxTelephoneNo.Location = new System.Drawing.Point(502, 198);
            this.textBoxTelephoneNo.Name = "textBoxTelephoneNo";
            this.textBoxTelephoneNo.Size = new System.Drawing.Size(307, 20);
            this.textBoxTelephoneNo.TabIndex = 54;
            // 
            // textBoxForename
            // 
            this.textBoxForename.Location = new System.Drawing.Point(503, 94);
            this.textBoxForename.Name = "textBoxForename";
            this.textBoxForename.Size = new System.Drawing.Size(307, 20);
            this.textBoxForename.TabIndex = 53;
            // 
            // textBoxSurname
            // 
            this.textBoxSurname.Location = new System.Drawing.Point(502, 120);
            this.textBoxSurname.Name = "textBoxSurname";
            this.textBoxSurname.Size = new System.Drawing.Size(307, 20);
            this.textBoxSurname.TabIndex = 52;
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(502, 146);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(307, 20);
            this.textBoxAddress.TabIndex = 51;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(502, 172);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(307, 20);
            this.textBoxEmail.TabIndex = 50;
            // 
            // listBoxCustomers
            // 
            this.listBoxCustomers.FormattingEnabled = true;
            this.listBoxCustomers.Location = new System.Drawing.Point(3, 0);
            this.listBoxCustomers.Name = "listBoxCustomers";
            this.listBoxCustomers.Size = new System.Drawing.Size(406, 485);
            this.listBoxCustomers.TabIndex = 49;
            this.listBoxCustomers.DoubleClick += new System.EventHandler(this.listBoxCustomers_DoubleClick);
            // 
            // tabPageCoaches
            // 
            this.tabPageCoaches.Controls.Add(this.label27);
            this.tabPageCoaches.Controls.Add(this.textBoxPassMani);
            this.tabPageCoaches.Controls.Add(this.buttonPassMani);
            this.tabPageCoaches.Controls.Add(this.label26);
            this.tabPageCoaches.Controls.Add(this.CoachIDTB);
            this.tabPageCoaches.Controls.Add(this.buttonDeleteCoach);
            this.tabPageCoaches.Controls.Add(this.buttonAddCoach);
            this.tabPageCoaches.Controls.Add(this.label20);
            this.tabPageCoaches.Controls.Add(this.textBox5);
            this.tabPageCoaches.Controls.Add(this.label18);
            this.tabPageCoaches.Controls.Add(this.textBox4);
            this.tabPageCoaches.Controls.Add(this.listBoxCoaches);
            this.tabPageCoaches.Location = new System.Drawing.Point(4, 22);
            this.tabPageCoaches.Name = "tabPageCoaches";
            this.tabPageCoaches.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCoaches.Size = new System.Drawing.Size(819, 505);
            this.tabPageCoaches.TabIndex = 1;
            this.tabPageCoaches.Text = "Coaches";
            this.tabPageCoaches.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(477, 292);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(43, 13);
            this.label27.TabIndex = 84;
            this.label27.Text = "Tour ID";
            // 
            // textBoxPassMani
            // 
            this.textBoxPassMani.Location = new System.Drawing.Point(526, 289);
            this.textBoxPassMani.Name = "textBoxPassMani";
            this.textBoxPassMani.Size = new System.Drawing.Size(287, 20);
            this.textBoxPassMani.TabIndex = 83;
            // 
            // buttonPassMani
            // 
            this.buttonPassMani.Location = new System.Drawing.Point(526, 315);
            this.buttonPassMani.Name = "buttonPassMani";
            this.buttonPassMani.Size = new System.Drawing.Size(151, 23);
            this.buttonPassMani.TabIndex = 82;
            this.buttonPassMani.Text = "Create Passenger Manifest";
            this.buttonPassMani.UseVisualStyleBackColor = true;
            this.buttonPassMani.Click += new System.EventHandler(this.buttonPassMani_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(468, 166);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(52, 13);
            this.label26.TabIndex = 81;
            this.label26.Text = "Coach ID";
            // 
            // CoachIDTB
            // 
            this.CoachIDTB.Location = new System.Drawing.Point(526, 163);
            this.CoachIDTB.Name = "CoachIDTB";
            this.CoachIDTB.Size = new System.Drawing.Size(287, 20);
            this.CoachIDTB.TabIndex = 80;
            // 
            // buttonDeleteCoach
            // 
            this.buttonDeleteCoach.Location = new System.Drawing.Point(526, 189);
            this.buttonDeleteCoach.Name = "buttonDeleteCoach";
            this.buttonDeleteCoach.Size = new System.Drawing.Size(81, 23);
            this.buttonDeleteCoach.TabIndex = 79;
            this.buttonDeleteCoach.Text = "Delete Coach";
            this.buttonDeleteCoach.UseVisualStyleBackColor = true;
            this.buttonDeleteCoach.Click += new System.EventHandler(this.buttonDeleteCoach_Click);
            // 
            // buttonAddCoach
            // 
            this.buttonAddCoach.Location = new System.Drawing.Point(526, 100);
            this.buttonAddCoach.Name = "buttonAddCoach";
            this.buttonAddCoach.Size = new System.Drawing.Size(75, 23);
            this.buttonAddCoach.TabIndex = 78;
            this.buttonAddCoach.Text = "Add Coach";
            this.buttonAddCoach.UseVisualStyleBackColor = true;
            this.buttonAddCoach.Click += new System.EventHandler(this.buttonAddCoach_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(419, 77);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(103, 13);
            this.label20.TabIndex = 77;
            this.label20.Text = "Registration Number";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(526, 74);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(287, 20);
            this.textBox5.TabIndex = 76;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(457, 51);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(63, 13);
            this.label18.TabIndex = 72;
            this.label18.Text = "No of Seats";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(526, 48);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(287, 20);
            this.textBox4.TabIndex = 70;
            // 
            // listBoxCoaches
            // 
            this.listBoxCoaches.FormattingEnabled = true;
            this.listBoxCoaches.Location = new System.Drawing.Point(7, 10);
            this.listBoxCoaches.Name = "listBoxCoaches";
            this.listBoxCoaches.Size = new System.Drawing.Size(406, 485);
            this.listBoxCoaches.TabIndex = 69;
            this.listBoxCoaches.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxCoaches_MouseDoubleClick);
            // 
            // tabPageTickets
            // 
            this.tabPageTickets.Controls.Add(this.buttonFinReport);
            this.tabPageTickets.Controls.Add(this.BuyTicketTourID);
            this.tabPageTickets.Controls.Add(this.label25);
            this.tabPageTickets.Controls.Add(this.TicketTourID);
            this.tabPageTickets.Controls.Add(this.label19);
            this.tabPageTickets.Controls.Add(this.buttonTicketSearchCustomerID);
            this.tabPageTickets.Controls.Add(this.buttonTicketSearchTicketID);
            this.tabPageTickets.Controls.Add(this.buttonTicketSearchTourID);
            this.tabPageTickets.Controls.Add(this.textBoxTicketSearch);
            this.tabPageTickets.Controls.Add(this.button1);
            this.tabPageTickets.Controls.Add(this.label23);
            this.tabPageTickets.Controls.Add(this.TicketReservationIDTB);
            this.tabPageTickets.Controls.Add(this.textBox2);
            this.tabPageTickets.Controls.Add(this.label24);
            this.tabPageTickets.Controls.Add(this.dateTimePickerReservation);
            this.tabPageTickets.Controls.Add(this.label22);
            this.tabPageTickets.Controls.Add(this.reserveCustomerIDTB);
            this.tabPageTickets.Controls.Add(this.label21);
            this.tabPageTickets.Controls.Add(this.buttonClearReservation);
            this.tabPageTickets.Controls.Add(this.buttonReserveTicket);
            this.tabPageTickets.Controls.Add(this.listBoxTickets);
            this.tabPageTickets.Controls.Add(this.label11);
            this.tabPageTickets.Controls.Add(this.bookCustomerIDTB);
            this.tabPageTickets.Controls.Add(this.PricePaidTB);
            this.tabPageTickets.Controls.Add(this.buttonBookTicket);
            this.tabPageTickets.Controls.Add(this.label10);
            this.tabPageTickets.Location = new System.Drawing.Point(4, 22);
            this.tabPageTickets.Name = "tabPageTickets";
            this.tabPageTickets.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTickets.Size = new System.Drawing.Size(819, 505);
            this.tabPageTickets.TabIndex = 2;
            this.tabPageTickets.Text = "Tickets";
            this.tabPageTickets.UseVisualStyleBackColor = true;
            // 
            // buttonFinReport
            // 
            this.buttonFinReport.Location = new System.Drawing.Point(502, 432);
            this.buttonFinReport.Name = "buttonFinReport";
            this.buttonFinReport.Size = new System.Drawing.Size(245, 64);
            this.buttonFinReport.TabIndex = 90;
            this.buttonFinReport.Text = "Create Financal Report";
            this.buttonFinReport.UseVisualStyleBackColor = true;
            this.buttonFinReport.Click += new System.EventHandler(this.buttonFinReport_Click);
            // 
            // BuyTicketTourID
            // 
            this.BuyTicketTourID.Location = new System.Drawing.Point(508, 95);
            this.BuyTicketTourID.Name = "BuyTicketTourID";
            this.BuyTicketTourID.Size = new System.Drawing.Size(216, 20);
            this.BuyTicketTourID.TabIndex = 89;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(459, 98);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(43, 13);
            this.label25.TabIndex = 88;
            this.label25.Text = "Tour ID";
            // 
            // TicketTourID
            // 
            this.TicketTourID.Location = new System.Drawing.Point(508, 216);
            this.TicketTourID.Name = "TicketTourID";
            this.TicketTourID.Size = new System.Drawing.Size(216, 20);
            this.TicketTourID.TabIndex = 87;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(459, 219);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 13);
            this.label19.TabIndex = 86;
            this.label19.Text = "Tour ID";
            // 
            // buttonTicketSearchCustomerID
            // 
            this.buttonTicketSearchCustomerID.Location = new System.Drawing.Point(586, 37);
            this.buttonTicketSearchCustomerID.Name = "buttonTicketSearchCustomerID";
            this.buttonTicketSearchCustomerID.Size = new System.Drawing.Size(78, 39);
            this.buttonTicketSearchCustomerID.TabIndex = 85;
            this.buttonTicketSearchCustomerID.Text = "Search by Customer ID";
            this.buttonTicketSearchCustomerID.UseVisualStyleBackColor = true;
            this.buttonTicketSearchCustomerID.Click += new System.EventHandler(this.buttonTicketSearchCustomerID_Click);
            // 
            // buttonTicketSearchTicketID
            // 
            this.buttonTicketSearchTicketID.Location = new System.Drawing.Point(502, 37);
            this.buttonTicketSearchTicketID.Name = "buttonTicketSearchTicketID";
            this.buttonTicketSearchTicketID.Size = new System.Drawing.Size(78, 39);
            this.buttonTicketSearchTicketID.TabIndex = 84;
            this.buttonTicketSearchTicketID.Text = "Search by Ticket ID";
            this.buttonTicketSearchTicketID.UseVisualStyleBackColor = true;
            this.buttonTicketSearchTicketID.Click += new System.EventHandler(this.buttonTicketSearchTicketID_Click);
            // 
            // buttonTicketSearchTourID
            // 
            this.buttonTicketSearchTourID.Location = new System.Drawing.Point(418, 37);
            this.buttonTicketSearchTourID.Name = "buttonTicketSearchTourID";
            this.buttonTicketSearchTourID.Size = new System.Drawing.Size(78, 39);
            this.buttonTicketSearchTourID.TabIndex = 83;
            this.buttonTicketSearchTourID.Text = "Search by Tour ID";
            this.buttonTicketSearchTourID.UseVisualStyleBackColor = true;
            this.buttonTicketSearchTourID.Click += new System.EventHandler(this.buttonTicketSearchTourID_Click);
            // 
            // textBoxTicketSearch
            // 
            this.textBoxTicketSearch.Location = new System.Drawing.Point(417, 11);
            this.textBoxTicketSearch.Name = "textBoxTicketSearch";
            this.textBoxTicketSearch.Size = new System.Drawing.Size(307, 20);
            this.textBoxTicketSearch.TabIndex = 82;
            this.textBoxTicketSearch.Text = "Search by Customer/Ticket/Tour ID";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(508, 393);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 23);
            this.button1.TabIndex = 81;
            this.button1.Text = "Pay For Reserved Ticket";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(447, 370);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(55, 13);
            this.label23.TabIndex = 80;
            this.label23.Text = "Price Paid";
            // 
            // TicketReservationIDTB
            // 
            this.TicketReservationIDTB.Location = new System.Drawing.Point(508, 341);
            this.TicketReservationIDTB.Name = "TicketReservationIDTB";
            this.TicketReservationIDTB.Size = new System.Drawing.Size(216, 20);
            this.TicketReservationIDTB.TabIndex = 77;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(508, 367);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(216, 20);
            this.textBox2.TabIndex = 79;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(451, 344);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(51, 13);
            this.label24.TabIndex = 78;
            this.label24.Text = "Ticket ID";
            // 
            // dateTimePickerReservation
            // 
            this.dateTimePickerReservation.Location = new System.Drawing.Point(508, 268);
            this.dateTimePickerReservation.Name = "dateTimePickerReservation";
            this.dateTimePickerReservation.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerReservation.TabIndex = 76;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(416, 274);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(86, 13);
            this.label22.TabIndex = 75;
            this.label22.Text = "Reservation End";
            // 
            // reserveCustomerIDTB
            // 
            this.reserveCustomerIDTB.Location = new System.Drawing.Point(508, 242);
            this.reserveCustomerIDTB.Name = "reserveCustomerIDTB";
            this.reserveCustomerIDTB.Size = new System.Drawing.Size(216, 20);
            this.reserveCustomerIDTB.TabIndex = 73;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(437, 245);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 13);
            this.label21.TabIndex = 74;
            this.label21.Text = "Customer ID";
            // 
            // buttonClearReservation
            // 
            this.buttonClearReservation.Location = new System.Drawing.Point(648, 393);
            this.buttonClearReservation.Name = "buttonClearReservation";
            this.buttonClearReservation.Size = new System.Drawing.Size(100, 23);
            this.buttonClearReservation.TabIndex = 72;
            this.buttonClearReservation.Text = "Clear Reservation";
            this.buttonClearReservation.UseVisualStyleBackColor = true;
            this.buttonClearReservation.Click += new System.EventHandler(this.buttonClearReservation_Click);
            // 
            // buttonReserveTicket
            // 
            this.buttonReserveTicket.Location = new System.Drawing.Point(508, 294);
            this.buttonReserveTicket.Name = "buttonReserveTicket";
            this.buttonReserveTicket.Size = new System.Drawing.Size(88, 23);
            this.buttonReserveTicket.TabIndex = 71;
            this.buttonReserveTicket.Text = "Reserve Ticket";
            this.buttonReserveTicket.UseVisualStyleBackColor = true;
            this.buttonReserveTicket.Click += new System.EventHandler(this.buttonReserveTicket_Click);
            // 
            // listBoxTickets
            // 
            this.listBoxTickets.FormattingEnabled = true;
            this.listBoxTickets.HorizontalScrollbar = true;
            this.listBoxTickets.Location = new System.Drawing.Point(6, 11);
            this.listBoxTickets.Name = "listBoxTickets";
            this.listBoxTickets.Size = new System.Drawing.Size(406, 485);
            this.listBoxTickets.TabIndex = 70;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(447, 150);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Price Paid";
            // 
            // bookCustomerIDTB
            // 
            this.bookCustomerIDTB.Location = new System.Drawing.Point(508, 121);
            this.bookCustomerIDTB.Name = "bookCustomerIDTB";
            this.bookCustomerIDTB.Size = new System.Drawing.Size(216, 20);
            this.bookCustomerIDTB.TabIndex = 31;
            // 
            // PricePaidTB
            // 
            this.PricePaidTB.Location = new System.Drawing.Point(508, 147);
            this.PricePaidTB.Name = "PricePaidTB";
            this.PricePaidTB.Size = new System.Drawing.Size(216, 20);
            this.PricePaidTB.TabIndex = 33;
            // 
            // buttonBookTicket
            // 
            this.buttonBookTicket.Location = new System.Drawing.Point(508, 173);
            this.buttonBookTicket.Name = "buttonBookTicket";
            this.buttonBookTicket.Size = new System.Drawing.Size(75, 23);
            this.buttonBookTicket.TabIndex = 30;
            this.buttonBookTicket.Text = "Buy Ticket";
            this.buttonBookTicket.UseVisualStyleBackColor = true;
            this.buttonBookTicket.Click += new System.EventHandler(this.buttonBookTicket_Click_1);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(437, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Customer ID";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(61, 552);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Tour ID";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 611);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.richTextBoxTourDescription);
            this.Controls.Add(this.dateTimePickerTourDate);
            this.Controls.Add(this.TourIDTB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxBaseTicketPrice);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxCoachID);
            this.Controls.Add(this.textBoxTourName);
            this.Controls.Add(this.buttonDeleteTour);
            this.Controls.Add(this.buttonEditTour);
            this.Controls.Add(this.buttonCreateTour);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxTours);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Wynne\'s Tours";
            this.tabControl.ResumeLayout(false);
            this.tabPageCustomer.ResumeLayout(false);
            this.tabPageCustomer.PerformLayout();
            this.tabPageCoaches.ResumeLayout(false);
            this.tabPageCoaches.PerformLayout();
            this.tabPageTickets.ResumeLayout(false);
            this.tabPageTickets.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxTours;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCreateTour;
        private System.Windows.Forms.Button buttonEditTour;
        private System.Windows.Forms.Button buttonDeleteTour;
        private System.Windows.Forms.TextBox textBoxTourName;
        private System.Windows.Forms.TextBox textBoxCoachID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxBaseTicketPrice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TourIDTB;
        private System.Windows.Forms.DateTimePicker dateTimePickerTourDate;
        private System.Windows.Forms.RichTextBox richTextBoxTourDescription;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageCustomer;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Button buttonCreateCustomer;
        private System.Windows.Forms.Button buttonEditCustomer;
        private System.Windows.Forms.Button buttonDeleteCustomer;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxTelephoneNo;
        private System.Windows.Forms.TextBox textBoxForename;
        private System.Windows.Forms.TextBox textBoxSurname;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.ListBox listBoxCustomers;
        private System.Windows.Forms.TabPage tabPageCoaches;
        private System.Windows.Forms.CheckBox checkBoxGoldClub;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCustomerSearch;
        private System.Windows.Forms.TextBox textBoxCustomerSearch;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ListBox listBoxCoaches;
        private System.Windows.Forms.TabPage tabPageTickets;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox bookCustomerIDTB;
        private System.Windows.Forms.TextBox PricePaidTB;
        private System.Windows.Forms.Button buttonBookTicket;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonDeleteCoach;
        private System.Windows.Forms.Button buttonAddCoach;
        private System.Windows.Forms.ListBox listBoxTickets;
        private System.Windows.Forms.TextBox reserveCustomerIDTB;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button buttonClearReservation;
        private System.Windows.Forms.Button buttonReserveTicket;
        private System.Windows.Forms.Button buttonTicketSearchTourID;
        private System.Windows.Forms.TextBox textBoxTicketSearch;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox TicketReservationIDTB;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button buttonTicketSearchCustomerID;
        private System.Windows.Forms.Button buttonTicketSearchTicketID;
        private System.Windows.Forms.DateTimePicker dateTimePickerReservation;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox TicketTourID;
        private System.Windows.Forms.TextBox BuyTicketTourID;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox CoachIDTB;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBoxPassMani;
        private System.Windows.Forms.Button buttonPassMani;
        private System.Windows.Forms.Button buttonFinReport;
    }
}

