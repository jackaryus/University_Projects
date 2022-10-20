unit main;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, ExtCtrls,
  StdCtrls, Data;

type

  { TfrmMain }

  TfrmMain = class(TForm)
    btnCreateNewFlyer: TButton;
    btnView: TButton;
    Edit1: TEdit;
    Image1: TImage;
    Label1: TLabel;
    lblForename: TLabel;
    lblSurname: TLabel;
    lblEmail: TLabel;
    ListBox1: TListBox;
    procedure btnCreateNewFlyerClick(Sender: TObject);
    procedure btnViewClick(Sender: TObject);
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  frmMain: TfrmMain;

implementation

{$R *.lfm}

{ TfrmMain }
uses createflyer, viewflyer;

procedure TfrmMain.btnCreateNewFlyerClick(Sender: TObject);
begin
  frmFlyer.Show; // Shows the FrmFlyer
  frmMain.hide;  // hides the current form
end;

procedure TfrmMain.btnViewClick(Sender: TObject);
begin
  frmViewFlyer.Show; // Shows the FrmFlyer
  self.hide;  // hides the current form
  frmviewflyer.flyername:=edit1.text;
end;

end.
