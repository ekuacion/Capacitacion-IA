object DMArriendo: TDMArriendo
  OldCreateOrder = False
  OnCreate = DataModuleCreate
  Left = 300
  Top = 180
  Height = 240
  Width = 360
  object dbArriendo: TDBISAMDatabase
    EngineVersion = '4.24 Build 1'
    DatabaseName = 'ARRIENDO'
    SessionName = 'Default'
    Left = 32
    Top = 16
  end
  object tblAutos: TDBISAMTable
    DatabaseName = 'ARRIENDO'
    EngineVersion = '4.24 Build 1'
    TableName = 'AUTOS'
    Left = 32
    Top = 72
  end
  object tblClientes: TDBISAMTable
    DatabaseName = 'ARRIENDO'
    EngineVersion = '4.24 Build 1'
    TableName = 'CLIENTES'
    Left = 112
    Top = 72
  end
  object tblArriendos: TDBISAMTable
    DatabaseName = 'ARRIENDO'
    EngineVersion = '4.24 Build 1'
    TableName = 'ARRIENDOS'
    Left = 192
    Top = 72
  end
  object qryGeneral: TDBISAMQuery
    DatabaseName = 'ARRIENDO'
    EngineVersion = '4.24 Build 1'
    Left = 32
    Top = 136
  end
end
