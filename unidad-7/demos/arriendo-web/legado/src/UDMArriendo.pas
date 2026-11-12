unit UDMArriendo;

interface

uses
  SysUtils, Classes, DB, dbisamtb;

type
  TDMArriendo = class(TDataModule)
    dbArriendo: TDBISAMDatabase;
    tblAutos: TDBISAMTable;
    tblClientes: TDBISAMTable;
    tblArriendos: TDBISAMTable;
    qryGeneral: TDBISAMQuery;
    procedure DataModuleCreate(Sender: TObject);
  private
  public
    function EjecutarSQL(const ASQL: string): Integer;
  end;

var
  DMArriendo: TDMArriendo;

implementation

{$R *.dfm}

procedure TDMArriendo.DataModuleCreate(Sender: TObject);
begin
  dbArriendo.Directory := 'C:\ARRIENDO\DATOS';
  dbArriendo.Connected := True;
  tblAutos.Open;
  tblClientes.Open;
  tblArriendos.Open;
end;

function TDMArriendo.EjecutarSQL(const ASQL: string): Integer;
begin
  qryGeneral.Close;
  qryGeneral.SQL.Text := ASQL;
  qryGeneral.ExecSQL;
  Result := qryGeneral.RowsAffected;
end;

end.
