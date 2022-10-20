unit login;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, sqldb, odbcconn, FileUtil, Forms, Controls, Graphics,
  Dialogs, ExtCtrls, StdCtrls, Data;

type

  { TfrmLogin }

  TfrmLogin = class(TForm)
    btnLogin: TButton;
    btnRegister: TButton;
    Label3: TLabel;
    txtUsername: TEdit;
    txtPassword: TEdit;
    Image1: TImage;
    Label1: TLabel;
    Label2: TLabel;
    procedure fillMain;
    procedure btnLoginClick(Sender: TObject);
    procedure btnRegisterClick(Sender: TObject);
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  frmLogin: TfrmLogin;

implementation

{$R *.lfm}

uses Register, main, createflyer;

procedure TfrmLogin.fillMain;
// this procedure accesses the database and loads the users details into frmMain
var
  strSQL: string;
begin
  with DataModule1 do
  begin
    //generate SQL
    strSQL := 'SELECT * FROM user WHERE username=';
    strSQL := strSQL + chr(39) + txtUsername.Text + chr(39);

    //clear any residual SQL
    if sqlUsers.Active then
      sqlUsers.Close;

    //set the SQL text
    sqlUsers.SQL.Text := strSQL;

    //execute the SQL
    sqlUsers.Open;

    //check if a record has been found
    if sqlUsers.RecordCount <> 0 then
    frmMain.lblForename.Caption := sqlUsers.FieldByName('forename').AsString;

    frmMain.lblSurname.Caption := sqlUsers.FieldByName('surname').AsString;
    frmMain.lblEmail.Caption := sqlUsers.FieldByName('Email').AsString;
  end;
end;

procedure TfrmLogin.btnLoginClick(Sender: TObject);
// this procedure logs the user in
var
  strSQL: string;
begin
  with DataModule1 do
  begin
    //generate SQL
    strSQL := 'SELECT username, password, customerID FROM user WHERE username=';
    strSQL := strSQL + chr(39) + txtUsername.Text + chr(39);

    //clear any residual SQL
    if sqlUsers.Active then
      sqlUsers.Close;

    //set the SQL text
    sqlUsers.SQL.Text := strSQL;

    //execute the SQL
    sqlUsers.Open;

    //check if a record has been found
    if sqlUsers.RecordCount = 0 then
      ShowMessage('invalid login details. please try again')
    else
    begin
      if txtPassword.Text = sqlUsers.FieldByName('password').AsString then
      begin
        frmflyer.customerID:= sqlUsers.FieldByName('customerID').AsInteger;
        frmMain.Show; //shows frmMain
        fillmain; //runs the fillmain procedure to fill the captions on the main form
        frmLogin.hide; //hides current form
      end
      else
        ShowMessage('invalid login details. please try again');
    end;

  end;
end;

procedure TfrmLogin.btnRegisterClick(Sender: TObject);
begin
  frmRegister.Show; // Shows the FrmRegister
  frmLogin.hide; // Makes FrmLogin invisible
end;


end.
