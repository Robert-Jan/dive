const defaultTheme = require('tailwindcss/defaultTheme')

module.exports = {
    content: ['/dive/src/Views/**/*.cshtml'],
    theme: {
        extend: {
            container: {
                center: true,
                padding: '2rem'
            },
            fontFamily: {
                sans: ['Inter var', ...defaultTheme.fontFamily.sans],
            },
        },
    },
    variants: {},
    plugins: [
        require('@tailwindcss/line-clamp'),
        require('@tailwindcss/forms'),
    ],
}