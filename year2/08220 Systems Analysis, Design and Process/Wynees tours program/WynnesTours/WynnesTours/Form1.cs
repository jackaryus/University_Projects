using DatabaseLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WynnesTours
{
    public partial class MainForm : Form
    {
        // Creating a new instance of WynneDatabase, in order to access the methods within it, in a different class
        WynneDatabase l = new WynneDatabase();
        // Local sources for the databases
        List<Tour> tours;
        List<Customer> customers;
        List<Coach> coaches;
        List<Ticket> tickets;
        public MainForm()
        {
            InitializeComponent();

            // Shows what tours and what coaches exists at the outset
            tours = l.GetTours(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(9), false);
            customers = l.GetCustomers(null);
            coaches = l.GetCoaches();
            tickets = l.GetTicketsForTour(1);

            // Showing the lists in their respective listboxes
            listBoxTours.Items.Clear();
            for (int i = 0; i < tours.Count; i++)
            {
                listBoxTours.Items.Add(tours[i].Title);
            }

            listBoxCustomers.Items.Clear();
            for (int i = 0; i < customers.Count; i++)
            {
                listBoxCustomers.Items.Add("Name: " + customers[i].Title + " " + customers[i].FirstName + " " + customers[i].Surname + " ID: " + customers[i].ID);
            }

            listBoxCoaches.Items.Clear();
            for (int i = 0; i < coaches.Count; i++)
            {
                listBoxCoaches.Items.Add("ID: " + coaches[i].ID + " Reg: " + coaches[i].RegistrationNumber + " Seats: " + coaches[i].NumberOfSeats);
            }

            listBoxTickets.Items.Clear();
            for (int i = 0; i < tickets.Count; i++)
            {
                textBoxTicketSearch.Text = "1";
                listBoxTickets.Items.Add("ID: " + tickets[i].ID + " Sold: " + tickets[i].IsSold + " Reserved: " + tickets[i].IsReserved + " Reservation Expires: " + tickets[i].ReservationExpiryDate + " Price: " + tickets[i].BasePrice + " Tour ID: " + tickets[i].TourID);
            }

        }

        #region Tour

        // All tour methods
        private void buttonCreateTour_Click(object sender, EventArgs e)
        {
            try
            {
                l.CreateTour(textBoxTourName.Text, richTextBoxTourDescription.Text, dateTimePickerTourDate.Value, int.Parse(textBoxCoachID.Text), double.Parse(textBoxBaseTicketPrice.Text));
                tours = l.GetTours(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(9), false);
                //create tickets for tour
                Tour p = tours.Find(x => x.Title == textBoxTourName.Text);
                Coach c = coaches.Find(x => x.ID == int.Parse(textBoxCoachID.Text));
                
                for (int m = 0; m < c.NumberOfSeats; m++ )
                {
                    l.CreateTicket(p.ID);
                }

                listBoxTours.Items.Clear();
                for (int i = 0; i < tours.Count; i++)
                {
                    listBoxTours.Items.Add(tours[i].Title);
                }
                MessageBox.Show("Tour successfully created!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void buttonEditTour_Click(object sender, EventArgs e)
        {
            //edits a tour then updates the listbox to show
            try
            {

                l.ModifyTour(int.Parse(TourIDTB.Text),textBoxTourName.Text, richTextBoxTourDescription.Text, dateTimePickerTourDate.Value, int.Parse(textBoxCoachID.Text), double.Parse(textBoxBaseTicketPrice.Text));
                tours = l.GetTours(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(9), false);
                listBoxTours.Items.Clear();
                for (int i = 0; i < tours.Count; i++)
                {
                    listBoxTours.Items.Add(tours[i].Title);
                }
                MessageBox.Show("Tour successfully Edited!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void buttonDeleteTour_Click(object sender, EventArgs e)
        {
            //deletes a tour then updates the listbox to show
            try
            {
                l.DeleteTour(int.Parse(TourIDTB.Text));

                tours = l.GetTours(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(9), false);
                listBoxTours.Items.Clear();
                for (int i = 0; i < tours.Count; i++)
                {
                    listBoxTours.Items.Add(tours[i].Title);
                }
                MessageBox.Show("Tour successfully Deleted!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void listBoxTours_DoubleClick(object sender, EventArgs e)
        {
            // when you double click on an item in a listbox it displays it's information on the textboxes
            int index = listBoxTours.SelectedIndex;

            try
            {
                Tour p = tours[index];
                textBoxTourName.Text = p.Title;
                richTextBoxTourDescription.Text = p.Description;
                dateTimePickerTourDate.Value = p.DepartureDateTime;
                textBoxCoachID.Text = p.CoachID.ToString();
                textBoxBaseTicketPrice.Text = p.BaseTicketPrice.ToString();
                TourIDTB.Text = p.ID.ToString();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        #endregion

        #region Customer
        // All customer methods
        private void buttonCreateCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                //Adds a customer then updates the textbox to show all the customers
                l.AddCustomer(textBoxTitle.Text, textBoxForename.Text, textBoxSurname.Text, textBoxAddress.Text, textBoxEmail.Text, textBoxTelephoneNo.Text);

                customers = l.GetCustomers(null);
                listBoxCustomers.Items.Clear();
                for (int i = 0; i < customers.Count; i++)
                {
                    listBoxCustomers.Items.Add("Name: " + customers[i].Title + " " + customers[i].FirstName + " " + customers[i].Surname + " ID: " + customers[i].ID);
                }
                MessageBox.Show("Customer successfully Added!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void buttonEditCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                //modifies the customer then updates the listbox to show that
                l.ModifyCustomer(int.Parse(textBoxCustomerSearch.Text), textBoxTitle.Text, textBoxForename.Text, textBoxSurname.Text, textBoxAddress.Text, textBoxEmail.Text, textBoxTelephoneNo.Text);

                if (checkBoxGoldClub.Checked == true)
                {
                    Customer c = customers.Find(x => x.ID == int.Parse(textBoxCustomerSearch.Text));
                    c.IsGoldClubMember = true;
                }
                
                customers = l.GetCustomers(null);

                listBoxCustomers.Items.Clear();
                for (int i = 0; i < customers.Count; i++)
                {
                    listBoxCustomers.Items.Add("Name: " + customers[i].Title + " " + customers[i].FirstName + " " + customers[i].Surname + " ID: " + customers[i].ID);
                }

                MessageBox.Show("Customer successfully Edited!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void buttonDeleteCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                //deletes the customer then updates the listbox to show that
                l.DeleteCustomer(int.Parse(textBoxCustomerSearch.Text));

                customers = l.GetCustomers(null);
                listBoxCustomers.Items.Clear();

                for (int i = 0; i < customers.Count; i++)
                {
                    listBoxCustomers.Items.Add("Name: " + customers[i].Title + " " + customers[i].FirstName + " " + customers[i].Surname + " ID: " + customers[i].ID);
                }
                MessageBox.Show("Customer successfully Deleted");
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void CustomerSearch(object sender, EventArgs e)
        {
            // Finding an individual customer
            int m;

            try
            {
                l.GetCustomerById(int.Parse(textBoxCustomerSearch.Text));
                Customer A = l.GetCustomerById(int.Parse(textBoxCustomerSearch.Text));

                textBoxTitle.Text = A.Title;
                textBoxForename.Text = A.FirstName;
                textBoxSurname.Text = A.Surname;
                textBoxAddress.Text = A.Address;
                textBoxEmail.Text = A.Email;
                textBoxTelephoneNo.Text = A.TelephoneNumber;
                if (A.IsGoldClubMember == true)
                {
                    checkBoxGoldClub.Checked = true;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void listBoxCustomers_DoubleClick(object sender, EventArgs e)
        {
            // when you double click on an item in a listbox it displays it's information on the textboxes
            int index = listBoxCustomers.SelectedIndex;
            checkBoxGoldClub.Checked = false;
            try
            {
                Customer p = customers[index];

                textBoxCustomerSearch.Text = p.ID.ToString();
                textBoxTitle.Text = p.Title;
                textBoxForename.Text = p.FirstName;
                textBoxSurname.Text = p.Surname;
                textBoxAddress.Text = p.Address;
                textBoxEmail.Text = p.Email;
                textBoxTelephoneNo.Text = p.TelephoneNumber;

                if (p.IsGoldClubMember == true)
                {
                    checkBoxGoldClub.Checked = true;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        #endregion

        #region Tickets

        // All ticket methods
        private void buttonTicketSearchTicketID_Click(object sender, EventArgs e)
        {

            // Shows individual ticket
            try
            {
                Ticket m = l.GetTicketById(int.Parse(textBoxTicketSearch.Text));

                reserveCustomerIDTB.Text = m.CustomerId.ToString();
                dateTimePickerReservation.Value = m.ReservationExpiryDate;
                TicketReservationIDTB.Text = m.ID.ToString();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void buttonTicketSearchTourID_Click(object sender, EventArgs e)
        {
            try
            {
                //finds tickets for tour then updates the tickets listbox to show those tickets
                tickets = l.GetTicketsForTour(int.Parse(textBoxTicketSearch.Text));
                listBoxTickets.Items.Clear();

                for (int i = 0; i < tickets.Count; i++)
                {
                    listBoxTickets.Items.Add("ID: " + tickets[i].ID + " Sold: " + tickets[i].IsSold + " Reserved: " + tickets[i].IsReserved + " Reservation Expires: " + tickets[i].ReservationExpiryDate + " Price: " + tickets[i].SalePrice + " Tour ID: " + tickets[i].TourID);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void buttonTicketSearchCustomerID_Click(object sender, EventArgs e)
        {
            try
            {
                //finds all tickets purchased or reserved by a customer then updates the listbox to show them
                tickets = l.GetTicketsByCustomerId(int.Parse(textBoxTicketSearch.Text));

                listBoxTickets.Items.Clear();

                for (int i = 0; i < tickets.Count; i++)
                {
                    listBoxTickets.Items.Add("ID: " + tickets[i].ID + " Sold: " + tickets[i].IsSold + " Reserved: " + tickets[i].IsReserved + " Reservation Expires: " + tickets[i].ReservationExpiryDate + " Price: " + tickets[i].SalePrice + " Tour ID: " + tickets[i].TourID);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void buttonBookTicket_Click_1(object sender, EventArgs e)
        {
            try
            {
                //books tickets for a customer
                l.BuyTicket(int.Parse(BuyTicketTourID.Text), int.Parse(bookCustomerIDTB.Text), double.Parse(PricePaidTB.Text));

                tickets = l.GetTicketsForTour(int.Parse(textBoxTicketSearch.Text));

                listBoxTickets.Items.Clear();

                for (int i = 0; i < tickets.Count; i++)
                {
                    listBoxTickets.Items.Add("ID: " + tickets[i].ID + " Sold: " + tickets[i].IsSold + " Reserved: " + tickets[i].IsReserved + " Reservation Expires: " + tickets[i].ReservationExpiryDate + " Price: " + tickets[i].SalePrice + " Tour ID: " + tickets[i].TourID);
                }

                MessageBox.Show("Ticket successfully Booked!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void buttonReserveTicket_Click(object sender, EventArgs e)
        {
            try
            {
                //reserves a tickets for a customer
                l.ReserveTicket(int.Parse(TicketTourID.Text), int.Parse(reserveCustomerIDTB.Text), dateTimePickerReservation.Value);

                tickets = l.GetTicketsForTour(int.Parse(textBoxTicketSearch.Text));

                listBoxTickets.Items.Clear();

                for (int i = 0; i < tickets.Count; i++)
                {
                    listBoxTickets.Items.Add("ID: " + tickets[i].ID + " Sold: " + tickets[i].IsSold + " Reserved: " + tickets[i].IsReserved + " Reservation Expires: " + tickets[i].ReservationExpiryDate + " Price: " + tickets[i].SalePrice + " Tour ID: " + tickets[i].TourID);
                }

                MessageBox.Show("Ticket successfully Reserved!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //pays for a reserved ticket
                l.PayForReservedTicket(int.Parse(TicketReservationIDTB.Text), double.Parse(textBox2.Text));

                tickets = l.GetTicketsForTour(int.Parse(textBoxTicketSearch.Text));

                listBoxTickets.Items.Clear();

                for (int i = 0; i < tickets.Count; i++)
                {
                    listBoxTickets.Items.Add("ID: " + tickets[i].ID + " Sold: " + tickets[i].IsSold + " Reserved: " + tickets[i].IsReserved + " Reservation Expires: " + tickets[i].ReservationExpiryDate + " Price: " + tickets[i].SalePrice + " Tour ID: " + tickets[i].TourID);
                }

                MessageBox.Show("Reserved ticket successfully payed for.");
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        private void buttonClearReservation_Click(object sender, EventArgs e)
        {
            try
            {
                l.CancelTicketReservation(int.Parse(TicketReservationIDTB.Text));

                tickets = l.GetTicketsForTour(int.Parse(textBoxTicketSearch.Text));

                listBoxTickets.Items.Clear();

                for (int i = 0; i < tickets.Count; i++)
                {
                    listBoxTickets.Items.Add("ID: " + tickets[i].ID + " Sold: " + tickets[i].IsSold + " Reserved: " + tickets[i].IsReserved + " Reservation Expires: " + tickets[i].ReservationExpiryDate + " Price: " + tickets[i].SalePrice + " Tour ID: " + tickets[i].TourID);
                }

                MessageBox.Show("Reserved ticket Unreserved.");
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        #endregion

        #region Coaches

        // All coach methods
        private void buttonAddCoach_Click(object sender, EventArgs e)
        {
            try
            {
                //Adds coaches and updates the local list and list box to show the new coach
                l.AddCoach(int.Parse(textBox4.Text), textBox5.Text);
                coaches = l.GetCoaches();

                listBoxCoaches.Items.Clear();

                for (int i = 0; i < coaches.Count; i++)
                {
                    listBoxCoaches.Items.Add("ID: " + coaches[i].ID + " Reg: " + coaches[i].RegistrationNumber + " Seats: " + coaches[i].NumberOfSeats);
                }
                MessageBox.Show("Coach successfully Added!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void buttonDeleteCoach_Click(object sender, EventArgs e)
        {
            try
            {
                // Deletes the coach, updates the local list and refreshes the list box
                l.DeleteCoach(int.Parse(CoachIDTB.Text));
                coaches = l.GetCoaches();

                listBoxCoaches.Items.Clear();

                for (int i = 0; i < customers.Count; i++)
                {
                    listBoxCoaches.Items.Add("ID: " + coaches[i].ID + " Reg: " + coaches[i].RegistrationNumber + " Seats: " + coaches[i].NumberOfSeats);
                }
                MessageBox.Show("Coach successfully Deleted!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void listBoxCoaches_MouseDoubleClick(object sender, MouseEventArgs e)
        {
                        // when you double click on an item in a listbox it displays it's information on the textboxes
            int index = listBoxCoaches.SelectedIndex;

            try
            {
                Coach p = coaches[index];

                textBox4.Text = p.NumberOfSeats.ToString();
                textBox5.Text = p.RegistrationNumber;
                CoachIDTB.Text = p.ID.ToString();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        #endregion

        #region Report & Manifest
        private void buttonPassMani_Click(object sender, EventArgs e)
        {
            // Prints a passanger manifest in a text document for coach driver

            try
            {
                tickets = l.GetTicketsForTour(int.Parse(textBoxPassMani.Text));

                StreamWriter r = new StreamWriter("Manifest.txt");

                for (int i = 0; i<tickets.Count; i++)
                {
                    r.WriteLine("ID: " + tickets[i].ID + " Sold: " + tickets[i].IsSold + " Price: " + tickets[i].SalePrice + " Tour ID: " + tickets[i].TourID + " Customer ID: " + tickets[i].CustomerId);
                }

                r.Close();

                //opens the file in a program of choice
                Process.Start("Manifest.txt");

            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void buttonFinReport_Click(object sender, EventArgs e)
        {
            try
            {

                int a = 0;
                int b = 0;
                double z = 0;

                for (int c = 0; c < tours.Count; c++)
                {
                    tickets = l.GetTicketsForTour(tours[c].ID);

                    a = a + tickets.Count;
                    
                    for (int d = 0; d<tickets.Count; d++)
                    {
                        if (tickets[d].IsSold == true)
                        {
                            //b counts how many tickets have been sold
                            b++;
                            //z adds all the prices of each sold ticket up to show total revenue
                            z = z + tickets[d].BasePrice;
                        }
                    }

                }
                //Creating a financial report & summary through data in databases
                StreamWriter r = new StreamWriter("FinancialReport.txt");

                r.WriteLine("Financial Report");
                r.WriteLine("----------------");

                r.WriteLine("");
                r.WriteLine("Summary: Tickets sold: {0} Total Tickets: {1}",b,a );
                r.WriteLine("         Ticket revenue: £{0}", z);
                r.WriteLine("");

                for (int i = 0; i < tours.Count; i++)
                {
                    int k = 0;
                    double y = 0;
                    //update the ticket list so it shows the tickets for the tour on each specific round of the for loop
                    tickets = l.GetTicketsForTour(tours[i].ID);

                    for (int m = 0; m<tickets.Count; m++)
                    {   
                        if(tickets[m].IsSold == true)
                        {
                            //k counts how many tickets have been sold for the tour
                            k++;
                            //y adds all the prices of each sold ticket up to show total revenue for thetour
                            y = y + tickets[m].BasePrice;
                        }
                    }

                    r.WriteLine(tours[i].Title + "    " + "Tickets sold: " + k + " Total tickets: " + tickets.Count);
                    r.WriteLine("                   Tour revenue: £{0}", y);
                    r.WriteLine();
                }

                r.Close();
                //opens the file in a program of choice
                Process.Start("FinancialReport.txt");

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        #endregion
    }

}
