using UnityEngine;

//Interfaz de estado
public interface IState
{
    void Enter(); // Al entrar en el estado
    void Execute(); // Lógica continua llamada desde Update()
    void Exit(); // Al salir del estado
}
