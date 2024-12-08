function enableOTPAutoJump() {
    const otpInputs = document.querySelectorAll('.otp-input');

    otpInputs.forEach((input, index) => {
        input.addEventListener('input', (e) => {
            if (e.target.value.length === 1) {
                otpInputs[index + 1].focus();
            }
        });
    });
}