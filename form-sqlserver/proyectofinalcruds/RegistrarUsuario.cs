using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace proyectofinalcruds
{
    public partial class RegistrarUsuario : Form
    {
        // Cadena de conexión a la base de datos
        private string connectionString = "Server=FATY; Database=ClinicaDB; Integrated Security=True";

        public RegistrarUsuario()
        {
            InitializeComponent();
            CargarDatos();  // Cargar los datos al iniciar el formulario
        }

        // Método para cargar los datos en el DataGridView
        private void CargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Id, Nombre, Rol, Contrasena FROM usuarios";  // Ajusta las columnas a tus necesidades
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Asignar los datos al DataGridView
                    dgvUsuarios.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos: " + ex.Message);
                }
            }
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)  // Verifica que se haya hecho clic en una fila válida
            {
                // Obtén la fila seleccionada
                DataGridViewRow row = dgvUsuarios.Rows[e.RowIndex];

                // Asigna los valores de las celdas a los campos de texto correspondientes
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtRol.Text = row.Cells["Rol"].Value.ToString();
                txtContrasena.Text = row.Cells["Contrasena"].Value.ToString();
            }
        }


        // Evento al hacer clic en un registro del DataGridView
        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)  // Verifica que se haya hecho clic en una fila válida
            {
                DataGridViewRow row = dgvUsuarios.Rows[e.RowIndex];
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();  // Asigna los valores a los campos de texto
                txtRol.Text = row.Cells["Rol"].Value.ToString();
                txtContrasena.Text = row.Cells["Contrasena"].Value.ToString();
            }
        }

        // Evento para registrar un nuevo usuario
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtNombre.Text;
            string rolUsuario = txtRol.Text;
            string contrasenaUsuario = txtContrasena.Text;

            if (RegistrarNuevoUsuario(nombreUsuario, rolUsuario, contrasenaUsuario))
            {
                MessageBox.Show("Usuario registrado correctamente.");
                CargarDatos();  // Recargar los datos
            }
            else
            {
                MessageBox.Show("Hubo un error al registrar el usuario.");
            }
        }

        // Método para registrar un nuevo usuario en la base de datos
        private bool RegistrarNuevoUsuario(string nombre, string rol, string contrasena)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO usuarios (Nombre, Rol, Contrasena) VALUES (@nombre, @rol, @contrasena)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@rol", rol);
                    command.Parameters.AddWithValue("@contrasena", contrasena);  // Contraseña sin cifrado
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar el usuario: " + ex.Message);
                    return false;
                }
            }
        }

        // Evento para actualizar un usuario
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtNombre.Text;
            string rolUsuario = txtRol.Text;
            string contrasenaUsuario = txtContrasena.Text;
            int usuarioId = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);  // Obtener el Id del usuario seleccionado

            if (ActualizarUsuario(usuarioId, nombreUsuario, rolUsuario, contrasenaUsuario))
            {
                MessageBox.Show("Usuario actualizado correctamente.");
                CargarDatos();  // Recargar los datos
            }
            else
            {
                MessageBox.Show("Hubo un error al actualizar el usuario.");
            }
        }

        // Método para actualizar un usuario en la base de datos
        private bool ActualizarUsuario(int id, string nombre, string rol, string contrasena)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE usuarios SET Nombre = @nombre, Rol = @rol, Contrasena = @contrasena WHERE Id = @id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@rol", rol);
                    command.Parameters.AddWithValue("@contrasena", contrasena);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el usuario: " + ex.Message);
                    return false;
                }
            }
        }

        // Evento para eliminar un usuario
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int usuarioId = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["Id"].Value);  // Obtener el Id del usuario seleccionado

            if (EliminarUsuario(usuarioId))
            {
                MessageBox.Show("Usuario eliminado correctamente.");
                CargarDatos();  // Recargar los datos
            }
            else
            {
                MessageBox.Show("Hubo un error al eliminar el usuario.");
            }
        }

        // Método para eliminar un usuario en la base de datos
        private bool EliminarUsuario(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM usuarios WHERE Id = @id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el usuario: " + ex.Message);
                    return false;
                }
            }
        }

        // Evento para regresar al formulario de login
        private void btnRegreso_Click(object sender, EventArgs e)
        {
            // Crear una instancia del formulario de login
            Form1 login = new Form1();

            // Mostrar el formulario de login
            login.Show();

            // Ocultar el formulario actual
            this.Hide();
        }
    }
}
