const defaultTheme = require('tailwindcss/defaultTheme')

module.exports = {
    purge: {
        content: ['./Views/**/*.cshtml'],
    },
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
        require('@tailwindcss/forms'),
    ],
}