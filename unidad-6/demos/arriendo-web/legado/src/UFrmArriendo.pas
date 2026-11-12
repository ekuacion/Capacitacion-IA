unit UFrmArriendo;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ComCtrls;

type
  TFrmArriendo = class(TForm)
    lblPatente: TLabel;
    edtPatente: TEdit;
    btnBuscarAuto: TButton;
    lblAuto: TLabel;
    lblRut: TLabel;
    edtRut: TEdit;
    btnBuscarCliente: TButton;
    lblCliente: TLabel;
    dtpSalida: TDateTimePicker;
    dtpPactada: TDateTimePicker;
    dtpDevolucion: TDateTimePicker;
    edtKm: TEdit;
    btnCalcular: TButton;
    lblTotal: TLabel;
    btnGrabar: TButton;
    procedure btnBuscarAutoClick(Sender: TObject);
    procedure btnBuscarClienteClick(Sender: TObject);
    procedure btnCalcularClick(Sender: TObject);
    procedure btnGrabarClick(Sender: TObject);
  private
    FPatente, FCategoria: string;
    FTarifaDia, FTotal: Double;
    FIdCliente, FCantArriendos: Integer;
    FNacimiento: TDateTime;
  public
  end;

var
  FrmArriendo: TFrmArriendo;

implementation

uses
  DateUtils, UDMArriendo, UGlobales, UTarifas, UUtilidades;

{$R *.dfm}

procedure TFrmArriendo.btnBuscarAutoClick(Sender: TObject);
begin
  with DMArriendo.qryGeneral do
  begin
    Close;
    SQL.Text := 'SELECT * FROM AUTOS WHERE PATENTE = ''' + edtPatente.Text + '''';
    Open;
    FPatente := FieldByName('PATENTE').AsString;
    FCategoria := FieldByName('CATEGORIA').AsString;
    FTarifaDia := FieldByName('TARIFA_DIA').AsFloat;
    lblAuto.Caption := FieldByName('MARCA').AsString + ' ' +
      FieldByName('MODELO').AsString;
    Close;
  end;
end;

procedure TFrmArriendo.btnBuscarClienteClick(Sender: TObject);
begin
  with DMArriendo.qryGeneral do
  begin
    Close;
    SQL.Text := 'SELECT C.ID_CLIENTE, C.NOMBRE, C.FECHA_NACIMIENTO, ' +
      '(SELECT COUNT(*) FROM ARRIENDOS A WHERE A.ID_CLIENTE = C.ID_CLIENTE) AS N ' +
      'FROM CLIENTES C WHERE C.RUT = ''' + LimpiaRut(edtRut.Text) + '''';
    Open;
    FIdCliente := FieldByName('ID_CLIENTE').AsInteger;
    FNacimiento := FieldByName('FECHA_NACIMIENTO').AsDateTime;
    FCantArriendos := FieldByName('N').AsInteger;
    lblCliente.Caption := FieldByName('NOMBRE').AsString;
    Close;
  end;
end;

procedure TFrmArriendo.btnCalcularClick(Sender: TObject);
var
  vDias, vAtraso: Integer;
  vBase, vRecargo, vSeguro: Double;
begin
  // dias de arriendo
  vDias := DiasArriendo(dtpSalida.DateTime, dtpPactada.DateTime);
  // devolucion atrasada
  vAtraso := 0;
  if dtpDevolucion.DateTime > dtpPactada.DateTime then
    vAtraso := HoursBetween(dtpDevolucion.DateTime, dtpPactada.DateTime);
  if vAtraso > 2 then
    vDias := vDias + 1;
  vBase := TarifaBase(FTarifaDia, vDias, dtpSalida.Date, FCantArriendos);
  // conductor joven
  vRecargo := 0;
  if EdadEnAnios(FNacimiento, dtpSalida.Date) < 25 then
    vRecargo := vBase * 0.20;
  // seguro
  vSeguro := 0;
  if FCategoria = 'L' then
    vSeguro := vDias * 12000;
  // total con kilometraje
  FTotal := vBase + vRecargo + vSeguro +
    CargoKilometraje(vDias, StrToIntDef(edtKm.Text, 0));
  lblTotal.Caption := IntToStr(vDias) + ' dias  ' + FormatoPesos(FTotal);
end;

procedure TFrmArriendo.btnGrabarClick(Sender: TObject);
begin
  try
    DMArriendo.EjecutarSQL('INSERT INTO ARRIENDOS (PATENTE, ID_CLIENTE, ' +
      'FECHA_SALIDA, FECHA_PACTADA, TOTAL, USUARIO) VALUES (''' + FPatente +
      ''', ' + IntToStr(FIdCliente) + ', ''' +
      FormatDateTime('yyyy-mm-dd hh:nn', dtpSalida.DateTime) + ''', ''' +
      FormatDateTime('yyyy-mm-dd hh:nn', dtpPactada.DateTime) + ''', ' +
      FloatToStr(FTotal) + ', ''' + gUsuario + ''')');
    ShowMessage('Arriendo grabado');
  except
  end;
end;

end.
