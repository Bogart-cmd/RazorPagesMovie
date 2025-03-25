/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.cshtml",
        "./Views/**/*.cshtml",
        "./wwwroot/**/*.html",
        "./Shared/**/*.cshtml",      
        "./_Layout/**/*.cshtml",     
        "./Components/**/*.cshtml",  
        "./**/*.cshtml"              
    ],
    theme: {
        extend: {},
    },
    plugins: [],
}
