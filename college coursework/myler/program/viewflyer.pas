unit viewflyer;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, ExtCtrls,
  StdCtrls, data;

type

  { Tfrmviewflyer }

  Tfrmviewflyer = class(TForm)
    Button1: TButton;
    Image3: TImage;
    Image4: TImage;
    Memo1: TMemo;
    Memo2: TMemo;
    Panel1: TPanel;
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { private declarations }
  public
  flyername: string;
  end;

var
  frmviewflyer: Tfrmviewflyer;

implementation

{$R *.lfm}

{ Tfrmviewflyer }
uses
  main;

procedure Tfrmviewflyer.FormCreate(Sender: TObject);
// this procedure accesses the database and loads the users flyer
var
  strSQL: string;
begin
  with DataModule1 do
  begin
    //generate SQL
    strSQL := 'SELECT * FROM flyer WHERE name=';
    strSQL := strSQL + chr(39) + frmMain.edit1.text + chr(39);

    //clear any residual SQL
    if sqlUsers.Active then
      sqlUsers.Close;

    //set the SQL text
    sqlUsers.SQL.Text := strSQL;

    //execute the SQL
    sqlUsers.Open;

    //check if a record has been found
  if sqlUsers.RecordCount <> 0 then
     begin
      //____________________________________________________________
   memo1.text:=sqlUsers.FieldByName('description').AsString;
   memo2.text:=sqlUsers.FieldByName('title').AsString;
   if (sqlUsers.FieldByName('Template').AsString = 'Template 1') then
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
   if sqlUsers.FieldByName('Template').AsString = 'Template 2' then
   begin
   frmViewFlyer.Panel1.color:=$00EED4B6;
   frmViewFlyer.memo1.Height:=327;
   frmViewFlyer.memo1.Width:=502;
   frmViewFlyer.memo1.Top:=253;
   frmViewFlyer.memo1.Left:=35;
   frmViewFlyer.Image3.Height:=145;
   frmViewFlyer.Image3.Width:=152;
   frmViewFlyer.Image3.Top:=80;
   frmViewFlyer.Image3.Left:=606;
   frmViewFlyer.Image4.Height:=200;
   frmViewFlyer.Image4.Width:=275;
   frmViewFlyer.Image4.Top:=309;
   frmViewFlyer.Image4.Left:=552;
   frmViewFlyer.memo2.Height:=107;
   frmViewFlyer.memo2.Width:=453;
   frmViewFlyer.memo2.Top:=74;
   frmViewFlyer.memo2.Left:=59;
   end;
   if sqlUsers.FieldByName('Template').AsString = 'Template 3' then
   begin
   frmViewFlyer.Panel1.color:=$00C3D8CF;
   frmViewFlyer.memo1.Height:=534;
   frmViewFlyer.memo1.Width:=445;
   frmViewFlyer.memo1.Top:=39;
   frmViewFlyer.memo1.Left:=382;
   frmViewFlyer.Image3.Height:=145;
   frmViewFlyer.Image3.Width:=152;
   frmViewFlyer.Image3.Top:=163;
   frmViewFlyer.Image3.Left:=129;
   frmViewFlyer.Image4.Height:=200;
   frmViewFlyer.Image4.Width:=275;
   frmViewFlyer.Image4.Top:=338;
   frmViewFlyer.Image4.Left:=72;
   frmViewFlyer.memo2.Height:=107;
   frmViewFlyer.memo2.Width:=334;
   frmViewFlyer.memo2.Top:=39;
   frmViewFlyer.memo2.Left:=37;
   end;
   if sqlUsers.FieldByName('Template').AsString = 'Template 4' then
   begin
   frmViewFlyer.Panel1.color:=$007FD6F8;
   frmViewFlyer.memo1.Height:=534;
   frmViewFlyer.memo1.Width:=486;
   frmViewFlyer.memo1.Top:=43;
   frmViewFlyer.memo1.Left:=40;
   frmViewFlyer.Image3.Height:=145;
   frmViewFlyer.Image3.Width:=152;
   frmViewFlyer.Image3.Top:=133;
   frmViewFlyer.Image3.Left:=549;
   frmViewFlyer.Image4.Height:=200;
   frmViewFlyer.Image4.Width:=275;
   frmViewFlyer.Image4.Top:=377;
   frmViewFlyer.Image4.Left:=549;
   frmViewFlyer.memo2.Height:=323;
   frmViewFlyer.memo2.Width:=94;
   frmViewFlyer.memo2.Top:=43;
   frmViewFlyer.memo2.Left:=730;
   end;

      //____________________________________________________________

   image3.picture.loadfromfile(sqlUsers.FieldByName('logo').AsString);
   image4.picture.loadfromfile(sqlUsers.FieldByName('picture').AsString);

   end
    else
    ShowMessage('No flyer with that name has been found. please enter a valid name');
  end;
end;

procedure Tfrmviewflyer.Button1Click(Sender: TObject);
begin
  self.hide;
  frmMain.show;
end;


end.

