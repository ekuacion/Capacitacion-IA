unit UTarifas;

{ tarifas de arriendo }

interface

function DiasArriendo(ASalida, ADevolucion: TDateTime): Integer;
function FactorTemporada(AFecha: TDateTime): Double;
function FactorFrecuente(ACantArriendos: Integer): Double;
function TarifaBase(ATarifaDia: Double; ADias: Integer; AFechaSalida: TDateTime;
  ACantArriendos: Integer): Double;
function CargoKilometraje(ADias, AKmEstimados: Integer): Double;

implementation

uses
  SysUtils, DateUtils;

{ dias completos entre salida y devolucion, minimo 1 }
function DiasArriendo(ASalida, ADevolucion: TDateTime): Integer;
begin
  Result := DaysBetween(DateOf(ADevolucion), DateOf(ASalida));
  if Result < 1 then
    Result := 1;
end;

{ verano }
function FactorTemporada(AFecha: TDateTime): Double;
begin
  if MonthOf(AFecha) in [12, 1, 2] then
    Result := 1.15
  else
    Result := 1;
end;

function FactorFrecuente(ACantArriendos: Integer): Double;
begin
  if ACantArriendos >= 5 then
    Result := 0.90
  else
    Result := 1;
end;

function TarifaBase(ATarifaDia: Double; ADias: Integer; AFechaSalida: TDateTime;
  ACantArriendos: Integer): Double;
begin
  Result := ATarifaDia * ADias;
  Result := Result * FactorTemporada(AFechaSalida);
  Result := Result * FactorFrecuente(ACantArriendos);
  Result := Round(Result);
end;

{ arriendos largos tienen km libre }
function CargoKilometraje(ADias, AKmEstimados: Integer): Double;
begin
  if ADias > 7 then
    Result := 0
  else if AKmEstimados > ADias * 200 then
    Result := (AKmEstimados - ADias * 200) * 150
  else
    Result := 0;
end;

end.
