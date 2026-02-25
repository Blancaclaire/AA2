<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useCategoryStore } from '@/stores/CategoryStore';
import { useUserStore } from '@/stores/UserStore';
import { useCourseStore } from '@/stores/CoursesStore'

const userStore = useUserStore()
const categoryStore = useCategoryStore()
const courseStore = useCourseStore()

const isSubmitting = ref(false)
const submitSuccess = ref(false)


onMounted(() => {
    categoryStore.getCategories()
    userStore.getUsers()
})



const levelOptions = [
    { value: '', text: 'Selecciona un nivel' },
    { value: 'Beginner', text: 'Principiante' },
    { value: 'Intermediate', text: 'Intermedio' },
    { value: 'Advanced', text: 'Avanzado' },
]

const form = ref({
    title: '',
    description: '',
    instructor: '',
    price: '',
    durationHours: '',
    level: '',
    imageUrl: '',
    categoryId: ''
})

const enviarDatos = async () => {
    isSubmitting.value = true
    submitSuccess.value = false

    const dataToSubmit = {
        ...form.value,
        price: parseFloat(form.value.price) || 0,
        durationHours: parseInt(form.value.durationHours) || 0,
        categoryId: parseInt(form.value.categoryId as string) || 0,
        isPublished: true,
    }
    console.log('Antes de llamar al store:', dataToSubmit)
    const success = await courseStore.postCourse(dataToSubmit)
    console.log('Resultado:', success, 'Error:', courseStore.error)
    isSubmitting.value = false

    if (success) {
        submitSuccess.value = true
        await courseStore.getCourses()
        form.value = {
            title: '', description: '', instructor: '',
            price: '', durationHours: '', level: '',
            imageUrl: '', categoryId: ''
        }
        setTimeout(() => { submitSuccess.value = false }, 3000)
    }
}

</script>

<template>

    <b-card header="Crear un curso" header-bg-variant="primary" header-text-variant="white" class="shadow-sm">
        <b-form @submit.prevent="enviarDatos">

            <b-form-group label="Titulo:" label-for="input-title" class="mb-3">
                <b-form-input id="input-name" v-model="form.title" type="text" placeholder="Programacion Vue"
                    required></b-form-input>
            </b-form-group>

            <b-form-group label="Description:" label-for="input-description" class="mb-3">
                <b-form-input id="input-description" v-model="form.description" type="text" placeholder="Clara"
                    required></b-form-input>
            </b-form-group>

            <b-form-group label="Categoría:" label-for="input-category" class="mb-3">
                <b-form-select id="input-category" v-model="form.categoryId" required>
                    <option value="">Selecciona una categoría</option>
                    <option v-for="cat in categoryStore.categories" :key="cat.id" :value="cat.id">
                        {{ cat.name }}
                    </option>
                </b-form-select>
            </b-form-group>

            <b-form-group label="Instructor:" label-for="input-instructor" class="mb-3">
                <b-form-select id="input-instructor" v-model="form.instructor" required>
                    <option value="">Selecciona un instructor</option>
                    <option v-for="u in userStore.users.filter(u => u.role === 'instructor' || u.role === 'admin')"
                        :key="u.id" :value="u.name">
                        {{ u.name }}
                    </option>
                </b-form-select>
            </b-form-group>

            <b-form-group label="Precio:" label-for="input-price" class="mb-3">
                <b-form-input id="input-price" v-model="form.price" type="number" placeholder="14.99"
                    required></b-form-input>
            </b-form-group>

            <b-form-group label="Duración (horas):" label-for="input-duration" class="mb-3">
                <b-form-input id="input-duration" v-model="form.durationHours" type="number" placeholder="10"
                    required></b-form-input>
            </b-form-group>

            <b-form-group label="Nivel:" label-for="input-level" class="mb-3">
                <b-form-select id="input-level" v-model="form.level" :options="levelOptions" required />
            </b-form-group>

            <b-button type="submit" :variant="submitSuccess ? 'success' : 'primary'" :disabled="isSubmitting"
                class="w-100 mt-3">
                <span v-if="isSubmitting">
                    <b-spinner small class="me-1"></b-spinner>
                    Añadiendo...
                </span>
                <span v-else-if="submitSuccess">
                    ¡Curso añadido!
                </span>
                <span v-else>
                    Añadir
                </span>
            </b-button>

        </b-form>
    </b-card>
</template>