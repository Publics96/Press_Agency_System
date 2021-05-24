const openModalButtons = document.querySelectorAll('[data-model-target]')
const closeModalButtons = document.querySelectorAll('[data-close-button]')
const overlay = document.getElementById('overlay-login')

openModalButtons.forEach(button => {
    button.addEventListener('click', (_) => {
        const modal = document.querySelector(button.dataset.modelTarget)
        openModal(modal)
    })
})

overlay.addEventListener('click', (_) => {
    const modals = document.querySelectorAll('.model.active')
    modals.forEach(modal => {
        closeModal(modal)
    })
})

closeModalButtons.forEach(button => {
    button.addEventListener('click', (_) => {
        const modal = button.closest('.model')
        closeModal(modal)
    })
})

function openModal(modal) {
    if (modal == null) return
    modal.classList.add('active')
    overlay.classList.add('active')
}

function closeModal(modal) {
    if (modal == null) return
    modal.classList.remove('active')
    overlay.classList.remove('active')
}