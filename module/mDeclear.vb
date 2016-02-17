Module mDeclear
    Public chara_centeroffset As Point

    Public Cam As New cCamera
    Public Curmap As cMap

    Public ResW As Int16 = 400, ResH As Int16 = 300

    Public bStop As Boolean = False

    Public Sys_cTex_Tsymbol As cTex
    Public Sys_cTex_Dmk16 As cTex
    Public Sys_cTex_Dmk8 As cTex
    Public Sys_cTex_Dmk5 As cTex

    Public Col_TextPop As New Collection

    Public Col_charaDmk As New Collection
    Public Col_enemyDmk As New Collection

    Public Col_Chara As New Collection
    Public Col_Enemy As New Collection
    Public iKey As Integer = 0

    Public iCheckNoticeProc As Integer = 1
    Public iEnemyArrayProc As Integer, iEnemyArrayCount As Integer
    Public vEnemyCollideBuffer() As PointF
    Public iEnemyCollideBufferProc As Integer

    Public mISrenderer As New cISrenderer

    Public SE(48) As cSound
    Public SEnames As Hashtable

    'Enum DIRECTION
    ' 7   8   9
    '     ^
    ' 4 < 5 > 6
    '     v
    ' 1   2   3
    ' 0 = none
    Enum DIRECTION
        none = 5
        up = 8
        down = 2
        left = 4
        right = 6
        upleft = 7
        upright = 9
        downleft = 1
        downright = 3
        unknown = 0
    End Enum

    Public Key_up As Int16 = Microsoft.DirectX.DirectInput.Key.W
    Public Key_down As Int16 = Microsoft.DirectX.DirectInput.Key.S
    Public Key_left As Int16 = Microsoft.DirectX.DirectInput.Key.A
    Public Key_right As Int16 = Microsoft.DirectX.DirectInput.Key.D
    Public Key_shoot As Int16 = Microsoft.DirectX.DirectInput.Key.NumPad4
    Public Key_skill1 As Int16 = Microsoft.DirectX.DirectInput.Key.NumPad5
    Public Key_skill2 As Int16 = Microsoft.DirectX.DirectInput.Key.NumPad6
    Public Key_fixDirection As Int16 = Microsoft.DirectX.DirectInput.Key.Space

    Public Debug_enemymoverecord(5) As PointF


End Module
