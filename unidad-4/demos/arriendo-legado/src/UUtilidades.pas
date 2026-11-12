unit UUtilidades;

interface

function EdadEnAnios(ANacimiento, AFecha: TDateTime): Integer;
function FormatoPesos(AValor: Double): string;
function LimpiaRut(const ARut: string): string;

implementation

uses
  SysUtils;

function EdadEnAnios(ANacimiento, AFecha: TDateTime): Integer;
var
  a1, m1, d1, a2, m2, d2: Word;
begin
  DecodeDate(ANacimiento, a1, m1, d1);
  DecodeDate(AFecha, a2, m2, d2);
  Result := a2 - a1;
  if (m2 < m1) or ((m2 = m1) and (d2 < d1)) then
    Dec(Result);
end;

function FormatoPesos(AValor: Double): string;
begin
  Result := '$ ' + FormatFloat('#,##0', AValor);
end;

function LimpiaRut(const ARut: string): string;
begin
  Result := StringReplace(ARut, '.', '', [rfReplaceAll]);
  Result := UpperCase(Trim(Result));
end;

end.
