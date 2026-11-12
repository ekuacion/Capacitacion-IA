object FrmArriendo: TFrmArriendo
  Left = 220
  Top = 140
  BorderStyle = bsSingle
  Caption = 'Arriendo de autos'
  ClientHeight = 300
  ClientWidth = 520
  Color = clBtnFace
  Font.Name = 'MS Sans Serif'
  OldCreateOrder = False
  Position = poScreenCenter
  PixelsPerInch = 96
  TextHeight = 13
  object lblPatente: TLabel
    Left = 16
    Top = 16
    Caption = 'Patente'
  end
  object edtPatente: TEdit
    Left = 96
    Top = 12
    Width = 97
    TabOrder = 0
  end
  object btnBuscarAuto: TButton
    Left = 200
    Top = 10
    Caption = 'Buscar'
    TabOrder = 1
    OnClick = btnBuscarAutoClick
  end
  object lblAuto: TLabel
    Left = 288
    Top = 16
  end
  object lblRut: TLabel
    Left = 16
    Top = 48
    Caption = 'RUT'
  end
  object edtRut: TEdit
    Left = 96
    Top = 44
    Width = 97
    TabOrder = 2
  end
  object btnBuscarCliente: TButton
    Left = 200
    Top = 42
    Caption = 'Buscar'
    TabOrder = 3
    OnClick = btnBuscarClienteClick
  end
  object lblCliente: TLabel
    Left = 288
    Top = 48
  end
  object dtpSalida: TDateTimePicker
    Left = 96
    Top = 88
    Width = 150
    Kind = dtkDate
    TabOrder = 4
  end
  object dtpPactada: TDateTimePicker
    Left = 96
    Top = 120
    Width = 150
    Kind = dtkDate
    TabOrder = 5
  end
  object dtpDevolucion: TDateTimePicker
    Left = 96
    Top = 152
    Width = 150
    Kind = dtkDate
    TabOrder = 6
  end
  object edtKm: TEdit
    Left = 96
    Top = 184
    Width = 65
    TabOrder = 7
  end
  object btnCalcular: TButton
    Left = 96
    Top = 224
    Caption = 'Calcular'
    TabOrder = 8
    OnClick = btnCalcularClick
  end
  object lblTotal: TLabel
    Left = 200
    Top = 230
    Font.Style = [fsBold]
    ParentFont = False
  end
  object btnGrabar: TButton
    Left = 96
    Top = 260
    Caption = 'Grabar'
    TabOrder = 9
    OnClick = btnGrabarClick
  end
end
