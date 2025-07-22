import { exec } from  "child_process";

// URL a abrir
const url = "http://localhost:4200/";

// Ejecutar el comando en Windows
exec(`start ${url}`, (err) => {
  if (err) {
    console.error("Error al abrir el navegador:", err);
  } else {
    console.log("Navegador abierto en:", url);
  }
});