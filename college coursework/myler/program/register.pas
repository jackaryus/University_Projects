unit Register;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, ExtCtrls,
  StdCtrls, Data;

type

  { TfrmRegister }

  TfrmRegister = class(TForm)
    btnRegisterSubmit: TButton;
    txtUsernameReg: TEdit;
    txtPasswordReg: TEdit;
    txtForename: TEdit;
    txtSurname: TEdit;
    txtEmail: TEdit;
    txtAddress1: TEdit;
    txtAddress2: TEdit;
    txtAddress3: TEdit;
    txtBusiness: TEdit;
    Image1: TImage;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    procedure btnRegisterSubmitClick(Sender: TObject);
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  frmRegister: TfrmRegister;

implementation

{$R *.lfm}

{ TfrmRegister }

uses login;

procedure TfrmRegister.btnRegisterSubmitClick(Sender: TObject);
// this procedure adds the details entered to the database
var
  strAddress, strSql: string;
begin
  with DataModule1 do
  begin
    //clear sql
    if sqlUsers.Active then
      sqlUsers.Close;
    //set sql
    strAddress := (txtAddress1.Text) + ', ' + (txtAddress2.Text) + ', ' + (txtAddress3.Text);
    strSql := 'INSERT INTO `user`(`username`, `password`, `forename`, `surname`, `email`, `address`, `business name`) VALUES('
      + chr(39) + txtUsernameReg.Text + chr(39) + ',' + chr(39) + txtPasswordReg.Text +
      chr(39) + ',' + chr(39) + txtForename.Text + chr(39) + ',' + chr(39) + txtSurname.Text +
      chr(39) + ',' + chr(39) + txtEmail.Text + chr(39) + ',' + chr(39) + strAddress + chr(39) + ',' + chr(39) +
      txtBusiness.Text + chr(39) + ')';
    SQLUsers.sql.Text := strSql;
    //execute the SQL
    sqlUsers.ExecSQL;
    MylerTransaction.Commit;
    self.hide; // hides current form
    frmLogin.Show; // goes back to FrmLogin
  end;
end;

end.
