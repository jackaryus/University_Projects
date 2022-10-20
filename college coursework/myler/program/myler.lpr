program myler;

{$mode objfpc}{$H+}

uses {$IFDEF UNIX} {$IFDEF UseCThreads}
  cthreads, {$ENDIF} {$ENDIF}
  Interfaces, // this includes the LCL widgetset
  Forms,
  login,
  Register,
  main,
  createflyer,
  Data, viewflyer;

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrmLogin, frmLogin);
  Application.CreateForm(TfrmRegister, frmRegister);
  Application.CreateForm(TfrmMain, frmMain);
  Application.CreateForm(TfrmFlyer, frmFlyer);
  Application.CreateForm(TDataModule1, DataModule1);
  Application.CreateForm(Tfrmviewflyer, frmviewflyer);
  Application.Run;
end.
