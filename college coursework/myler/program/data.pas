unit Data;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils, sqldb, odbcconn, FileUtil;

type

  { TDataModule1 }

  TDataModule1 = class(TDataModule)
    MylerConnection: TODBCConnection;
    MylerTransaction: TSQLTransaction;
    SQLUsers: TSQLQuery;
  private
    { private declarations }
  public
    { public declarations }
  end;

var
  DataModule1: TDataModule1;

implementation

 {$R *.lfm}

end.
