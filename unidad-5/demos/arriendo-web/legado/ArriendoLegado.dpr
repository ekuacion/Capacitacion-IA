program ArriendoLegado;

uses
  Forms,
  UDMArriendo in 'src\UDMArriendo.pas' {DMArriendo: TDataModule},
  UGlobales in 'src\UGlobales.pas',
  UUtilidades in 'src\UUtilidades.pas',
  UTarifas in 'src\UTarifas.pas',
  UFrmArriendo in 'src\UFrmArriendo.pas' {FrmArriendo};

{$R *.res}

begin
  Application.Initialize;
  Application.Title := 'Arriendo de autos';
  Application.CreateForm(TDMArriendo, DMArriendo);
  Application.CreateForm(TFrmArriendo, FrmArriendo);
  Application.Run;
end.
