unit createflyer;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, ExtCtrls,
  StdCtrls, Menus, data;

type

  { TfrmFlyer }

  TfrmFlyer = class(TForm)
    btnLogo: TButton;
    btnPicture: TButton;
    btnSave: TButton;
    ddTemplate: TComboBox;
    Edit1: TEdit;
    Image1: TImage;
    Image3: TImage;
    Image4: TImage;
    Label1: TLabel;
    Memo1: TMemo;
    Memo2: TMemo;
    OpenDialog1: TOpenDialog;
    Panel1: TPanel;
    procedure btnLogoClick(Sender: TObject);
    procedure btnPictureClick(Sender: TObject);
    procedure btnSaveClick(Sender: TObject);
    procedure ddTemplateChange(Sender: TObject);

  private
    { private declarations }
  public
    customerID:integer
  end;

var
  frmFlyer: TfrmFlyer;
  Fnamepic, Fnamelogo:string;

implementation

{$R *.lfm}

{ TfrmFlyer }

uses main;

procedure TfrmFlyer.ddTemplateChange(Sender: TObject);
//this procedure changes the size and positions of all the objects on
//the panel and the panel colour depending on the template selected
begin
   if ddTemplate.text='Template 1' then
   begin
   Panel1.color:=$008AA3E9;
   memo1.Height:=324;
   memo1.Width:=789;
   memo1.Top:=251;
   memo1.Left:=35;
   Image3.Height:=145;
   Image3.Width:=152;
   Image3.Top:=39;
   Image3.Left:=35;
   Image4.Height:=200;
   Image4.Width:=275;
   Image4.Top:=40;
   Image4.Left:=548;
   memo2.Height:=87;
   memo2.Width:=320;
   memo2.Top:=66;
   memo2.Left:=206;
   end;
   if ddTemplate.text='Template 2' then
   begin
   Panel1.color:=$00EED4B6;
   memo1.Height:=327;
   memo1.Width:=502;
   memo1.Top:=253;
   memo1.Left:=35;
   Image3.Height:=145;
   Image3.Width:=152;
   Image3.Top:=80;
   Image3.Left:=606;
   Image4.Height:=200;
   Image4.Width:=275;
   Image4.Top:=309;
   Image4.Left:=552;
   memo2.Height:=107;
   memo2.Width:=453;
   memo2.Top:=74;
   memo2.Left:=59;
   end;
   if ddTemplate.text='Template 3' then
   begin
   Panel1.color:=$00C3D8CF;
   memo1.Height:=534;
   memo1.Width:=445;
   memo1.Top:=39;
   memo1.Left:=382;
   Image3.Height:=145;
   Image3.Width:=152;
   Image3.Top:=163;
   Image3.Left:=129;
   Image4.Height:=200;
   Image4.Width:=275;
   Image4.Top:=338;
   Image4.Left:=72;
   memo2.Height:=107;
   memo2.Width:=334;
   memo2.Top:=39;
   memo2.Left:=37;
   end;
   if ddTemplate.text='Template 4' then
   begin
   Panel1.color:=$007FD6F8;
   memo1.Height:=534;
   memo1.Width:=486;
   memo1.Top:=43;
   memo1.Left:=40;
   Image3.Height:=145;
   Image3.Width:=152;
   Image3.Top:=133;
   Image3.Left:=549;
   Image4.Height:=200;
   Image4.Width:=275;
   Image4.Top:=377;
   Image4.Left:=549;
   memo2.Height:=323;
   memo2.Width:=94;
   memo2.Top:=43;
   memo2.Left:=730;
   end;
end;

procedure TfrmFlyer.btnLogoClick(Sender: TObject);
//this procedure opens a dialog box to load a logo into the postion
//required on the template
begin
  if opendialog1.execute then
  fnamelogo:=opendialog1.filename;
  image3.picture.loadfromfile(Fnamelogo);
end;

procedure TfrmFlyer.btnPictureClick(Sender: TObject);
//this procedure opens a dialog box to load a picture into the postion
//required on the template
begin
  if opendialog1.execute then
  fnamepic:=opendialog1.filename;
  image4.picture.loadfromfile(Fnamepic);
end;

procedure TfrmFlyer.btnSaveClick(Sender: TObject);
//this procedure saves the created flyer to the database
var strsql: string;
begin
 with DataModule1 do
  begin
    //clear sql
    if sqlUsers.Active then
      sqlUsers.Close;
    //set sql
    strSql := 'INSERT INTO flyer(CustomerID, name, template, description, title, picture, logo) VALUES('
      + chr(39) +inttostr(customerID)+ chr(39)+',' + chr(39) + edit1.Text + chr(39) + ',' + chr(39) + ddtemplate.Text +
      chr(39) + ',' + chr(39) + memo1.Text + chr(39) + ',' + chr(39) + memo2.Text +
      chr(39) + ',' + chr(34) + fnamepic + chr(34) + ',' + chr(34) + fnamelogo + chr(34) + ')';
    SQLUsers.sql.Text := strSql;
    //execute the SQL
    sqlUsers.ExecSQL;
    MylerTransaction.Commit;
    self.hide; // hides current form
    frmmain.Show; // goes back to FrmLogin
  end;
end;


end.
