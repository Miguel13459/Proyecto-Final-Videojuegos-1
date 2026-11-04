using System;
using System.Collections.Generic;

[Serializable]
public class DialogueData
{
    public string introduccion;
    public string lugar;
    public string final;
}

[Serializable]
public class DialogoClientes
{
    public string dialogo;
}

[Serializable]
public class DialogosArchivoData
{
    public List<DialogueData> jefeJackson;
    public List<DialogoClientes> clientes;
}