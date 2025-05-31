// Admin Dashboard Main Script
document.addEventListener('DOMContentLoaded', function() {
    // Check authentication
    checkAuth();
    
    // Initialize the dashboard
    initDashboard();
});

// Global variables
let products = [];
let categories = [];
let currentLanguage = 'en'; // Default admin language

// Check if user is authenticated
function checkAuth() {
    const token = localStorage.getItem('adminToken');
    if (!token) {
        window.location.href = '/admin/login.html';
    }
}

function initDashboard() {
    setupNavigation();
    setupLanguageToggle();
    setupLogout();
    
    // Load initial data
    loadDashboardStats();
    loadProducts();
    loadCategories();
    
    // Setup forms
    setupProductForms();
    setupCategoryForms();
    
    // Setup image previews
    setupImagePreviews();
}

// Setup navigation between sections
function setupNavigation() {
    const navLinks = document.querySelectorAll('.nav-link');
    
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            
            // Remove active class from all links
            navLinks.forEach(l => l.classList.remove('active'));
            
            // Add active class to clicked link
            this.classList.add('active');
            
            // Hide all sections
            document.querySelectorAll('[id$="Section"]').forEach(section => {
                section.style.display = 'none';
            });
            
            // Show selected section
            const sectionId = this.getAttribute('data-section') + 'Section';
            document.getElementById(sectionId).style.display = 'block';
        });
    });
}

// Setup language toggle
function setupLanguageToggle() {
    const langToggle = document.getElementById('languageToggle');
    if (!langToggle) return;

    langToggle.addEventListener('change', function() {
        currentLanguage = this.checked ? 'ar' : 'en';
        updateAdminLanguage();
    });
}

// Update admin interface language
function updateAdminLanguage() {
    // Update UI elements based on currentLanguage
    const elements = {
        'dashboardTitle': currentLanguage === 'ar' ? 'لوحة التحكم' : 'Dashboard',
        'productsTitle': currentLanguage === 'ar' ? 'المنتجات' : 'Products',
        'categoriesTitle': currentLanguage === 'ar' ? 'الفئات' : 'Categories',
        'addProductBtn': currentLanguage === 'ar' ? 'إضافة منتج' : 'Add Product',
        'addCategoryBtn': currentLanguage === 'ar' ? 'إضافة فئة' : 'Add Category',
        // Add more elements as needed
    };

    for (const [id, text] of Object.entries(elements)) {
        const element = document.getElementById(id);
        if (element) element.textContent = text;
    }

    // Update table headers
    updateTableHeaders();
}

function updateTableHeaders() {
    const productHeaders = {
        'id': currentLanguage === 'ar' ? 'ID' : 'ID',
        'name': currentLanguage === 'ar' ? 'الاسم' : 'Name',
        'price': currentLanguage === 'ar' ? 'السعر' : 'Price',
        'category': currentLanguage === 'ar' ? 'الفئة' : 'Category',
        'status': currentLanguage === 'ar' ? 'الحالة' : 'Status',
        'actions': currentLanguage === 'ar' ? 'الإجراءات' : 'Actions'
    };

    const categoryHeaders = {
        'id': currentLanguage === 'ar' ? 'ID' : 'ID',
        'name': currentLanguage === 'ar' ? 'الاسم' : 'Name',
        'description': currentLanguage === 'ar' ? 'الوصف' : 'Description',
        'actions': currentLanguage === 'ar' ? 'الإجراءات' : 'Actions'
    };

    // Update product table headers
    const productThs = document.querySelectorAll('#productsTableBody').closest('table')?.querySelectorAll('th');
    if (productThs) {
        productThs.forEach(th => {
            const key = th.dataset.key;
            if (key && productHeaders[key]) {
                th.textContent = productHeaders[key];
            }
        });
    }

    // Update category table headers
    const categoryThs = document.querySelectorAll('#categoriesTableBody').closest('table')?.querySelectorAll('th');
    if (categoryThs) {
        categoryThs.forEach(th => {
            const key = th.dataset.key;
            if (key && categoryHeaders[key]) {
                th.textContent = categoryHeaders[key];
            }
        });
    }
}

// Setup logout functionality
function setupLogout() {
    document.getElementById('logoutBtn').addEventListener('click', function() {
        localStorage.removeItem('adminToken');
        window.location.href = '/admin/login.html';
    });
}

// Load dashboard statistics
async function loadDashboardStats() {
    try {
        const [productsResponse, categoriesResponse] = await Promise.all([
            fetch('https://localhost:7018/api/products', {
                headers: getAuthHeaders()
            }),
            fetch('https://localhost:7018/api/categories', {
                headers: getAuthHeaders()
            })
        ]);
        
        if (!productsResponse.ok || !categoriesResponse.ok) {
            throw new Error(currentLanguage === 'ar' ? 
                'فشل تحميل إحصائيات لوحة التحكم' : 
                'Failed to load dashboard stats');
        }
        
        const products = await productsResponse.json();
        const categories = await categoriesResponse.json();
        
        document.getElementById('totalProducts').textContent = products.length;
        document.getElementById('totalCategories').textContent = categories.length;
    } catch (error) {
        console.error('Error loading dashboard stats:', error);
        showError(currentLanguage === 'ar' ? 
            'خطأ في تحميل إحصائيات لوحة التحكم' : 
            'Failed to load dashboard data');
    }
}

// Load products
async function loadProducts() {
    try {
        const response = await fetch('https://localhost:7018/api/products', {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error(currentLanguage === 'ar' ? 
                'فشل تحميل المنتجات' : 
                'Failed to load products');
        }
        
        products = await response.json();
        const tableBody = document.getElementById('productsTableBody');
        tableBody.innerHTML = '';
        
        products.forEach(product => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${product.id}</td>
                <td>
                    ${product.imageUrl ? `<img src="${product.imageUrl}" alt="${currentLanguage === 'ar' ? product.nameAr : product.name}" style="max-height: 50px; margin-right: 10px;">` : ''}
                    ${currentLanguage === 'ar' ? product.nameAr : product.name}
                </td>
                <td>${product.price.toFixed(2)}</td>
                <td>${currentLanguage === 'ar' ? (product.categoryNameAr || 'N/A') : (product.categoryName || 'N/A')}</td>
                <td>${product.isAvailable ? 
                    `<span class="badge bg-success">${currentLanguage === 'ar' ? 'متوفر' : 'Available'}</span>` : 
                    `<span class="badge bg-danger">${currentLanguage === 'ar' ? 'غير متوفر' : 'Unavailable'}</span>`}
                </td>
                <td>
                    <button class="btn btn-sm btn-primary edit-product" data-id="${product.id}">
                        ${currentLanguage === 'ar' ? 'تعديل' : 'Edit'}
                    </button>
                    <button class="btn btn-sm btn-danger delete-product" data-id="${product.id}">
                        ${currentLanguage === 'ar' ? 'حذف' : 'Delete'}
                    </button>
                </td>
            `;
            tableBody.appendChild(row);
        });
        
        // Add event listeners to edit/delete buttons
        document.querySelectorAll('.edit-product').forEach(button => {
            button.addEventListener('click', function() {
                const productId = this.getAttribute('data-id');
                editProduct(productId);
            });
        });
        
        document.querySelectorAll('.delete-product').forEach(button => {
            button.addEventListener('click', function() {
                const productId = this.getAttribute('data-id');
                const confirmMessage = currentLanguage === 'ar' ? 
                    'هل أنت متأكد أنك تريد حذف هذا المنتج؟' : 
                    'Are you sure you want to delete this product?';
                
                if (confirm(confirmMessage)) {
                    deleteProduct(productId);
                }
            });
        });
    } catch (error) {
        console.error('Error loading products:', error);
        showError(currentLanguage === 'ar' ? 
            'خطأ في تحميل المنتجات' : 
            'Failed to load products');
    }
}

// Load categories
async function loadCategories() {
    try {
        const response = await fetch('https://localhost:7018/api/categories', {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error(currentLanguage === 'ar' ? 
                'فشل تحميل الفئات' : 
                'Failed to load categories');
        }
        
        categories = await response.json();
        const tableBody = document.getElementById('categoriesTableBody');
        tableBody.innerHTML = '';
        
        categories.forEach(category => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${category.id}</td>
                <td>
                    ${category.imageUrl ? `<img src="${category.imageUrl}" alt="${currentLanguage === 'ar' ? category.nameAr : category.name}" style="max-height: 50px; margin-right: 10px;">` : ''}
                    ${currentLanguage === 'ar' ? category.nameAr : category.name}
                </td>
                <td>${currentLanguage === 'ar' ? category.descriptionAr : category.description}</td>
                <td>
                    <button class="btn btn-sm btn-primary edit-category" data-id="${category.id}">
                        ${currentLanguage === 'ar' ? 'تعديل' : 'Edit'}
                    </button>
                    <button class="btn btn-sm btn-danger delete-category" data-id="${category.id}">
                        ${currentLanguage === 'ar' ? 'حذف' : 'Delete'}
                    </button>
                </td>
            `;
            tableBody.appendChild(row);
        });
        
        // Add event listeners to edit/delete buttons
        document.querySelectorAll('.edit-category').forEach(button => {
            button.addEventListener('click', function() {
                const categoryId = this.getAttribute('data-id');
                editCategory(categoryId);
            });
        });
        
        document.querySelectorAll('.delete-category').forEach(button => {
            button.addEventListener('click', function() {
                const categoryId = this.getAttribute('data-id');
                const confirmMessage = currentLanguage === 'ar' ? 
                    'هل أنت متأكد أنك تريد حذف هذه الفئة؟' : 
                    'Are you sure you want to delete this category?';
                
                if (confirm(confirmMessage)) {
                    deleteCategory(categoryId);
                }
            });
        });
        
        // Also populate category dropdowns in product forms
        populateCategoryDropdowns(categories);
    } catch (error) {
        console.error('Error loading categories:', error);
        showError(currentLanguage === 'ar' ? 
            'خطأ في تحميل الفئات' : 
            'Failed to load categories');
    }
}

// Populate category dropdowns
function populateCategoryDropdowns(categories) {
    const productCategory = document.getElementById('productCategory');
    const editProductCategory = document.getElementById('editProductCategory');
    
    productCategory.innerHTML = '';
    editProductCategory.innerHTML = '';
    
    // Add default option
    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.textContent = currentLanguage === 'ar' ? 'اختر فئة' : 'Select a category';
    defaultOption.selected = true;
    defaultOption.disabled = true;
    productCategory.appendChild(defaultOption.cloneNode(true));
    editProductCategory.appendChild(defaultOption.cloneNode(true));
    
    // Add category options
    categories.forEach(category => {
        const option = document.createElement('option');
        option.value = category.id;
        option.textContent = currentLanguage === 'ar' ? category.nameAr : category.name;
        
        productCategory.appendChild(option.cloneNode(true));
        editProductCategory.appendChild(option.cloneNode(true));
    });
}



// Delete product
async function deleteProduct(productId) {
    try {
        const response = await fetch(`https://localhost:7018/api/products/${productId}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error(currentLanguage === 'ar' ? 
                'فشل حذف المنتج' : 
                'Failed to delete product');
        }
        
        // Refresh products
        loadProducts();
        loadDashboardStats();
        
        showSuccess(currentLanguage === 'ar' ? 
            'تم حذف المنتج بنجاح' : 
            'Product deleted successfully');
    } catch (error) {
        console.error('Error deleting product:', error);
        showError(currentLanguage === 'ar' ? 
            'فشل حذف المنتج' : 
            'Failed to delete product');
    }
}

// Setup category forms
function setupCategoryForms() {
    // Add category form
    document.getElementById('saveCategoryBtn').addEventListener('click', async function() {
        const form = document.getElementById('addCategoryForm');
        const formData = new FormData();
        
        // Add all fields
        formData.append('Name', document.getElementById('categoryName').value);
        formData.append('NameAr', document.getElementById('categoryNameAr').value);
        formData.append('Description', document.getElementById('categoryDescription').value);
        formData.append('DescriptionAr', document.getElementById('categoryDescriptionAr').value);
        formData.append('Image', document.getElementById('categoryImage').files[0]);
        
        try {
            const response = await fetch('https://localhost:7018/api/categories', {
                method: 'POST',
                headers: getAuthHeaders(false),
                body: formData
            });
            
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || (currentLanguage === 'ar' ? 
                    'فشل إضافة الفئة' : 
                    'Failed to add category'));
            }
            
            // Close modal and refresh categories
            bootstrap.Modal.getInstance(document.getElementById('addCategoryModal')).hide();
            form.reset();
            loadCategories();
            loadDashboardStats();
            
            showSuccess(currentLanguage === 'ar' ? 
                'تمت إضافة الفئة بنجاح' : 
                'Category added successfully');
        } catch (error) {
            console.error('Error adding category:', error);
            showError(currentLanguage === 'ar' ? 
                'فشل إضافة الفئة: ' + error.message : 
                'Failed to add category: ' + error.message);
        }
    });
    
    // Edit category form is handled in the editCategory function
}


// Setup product forms
function setupProductForms() {
    // Image preview for add product
    document.getElementById('productImage').addEventListener('change', function() {
        const preview = document.getElementById('productImagePreview');
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                preview.src = e.target.result;
                preview.style.display = 'block';
            }
            reader.readAsDataURL(file);
        }
    });

    // Save product
    document.getElementById('saveProductBtn').addEventListener('click', async function() {
        const form = document.getElementById('addProductForm');
        const formData = new FormData();
        
        // Add all fields
        formData.append('Name', document.getElementById('productName').value);
        formData.append('NameAr', document.getElementById('productNameAr').value);
        formData.append('Description', document.getElementById('productDescription').value);
        formData.append('DescriptionAr', document.getElementById('productDescriptionAr').value);
        formData.append('Price', document.getElementById('productPrice').value);
        formData.append('CategoryId', document.getElementById('productCategory').value);
        formData.append('Image', document.getElementById('productImage').files[0]);
        formData.append('IsAvailable', document.getElementById('productIsAvailable').checked);
        
        try {
            const response = await fetch('https://localhost:7018/api/products', {
                method: 'POST',
                headers: getAuthHeaders(false),
                body: formData
            });
            
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || (currentLanguage === 'ar' ? 
                    'فشل إضافة المنتج' : 
                    'Failed to add product'));
            }
            
            // Close modal and refresh products
            bootstrap.Modal.getInstance(document.getElementById('addProductModal')).hide();
            form.reset();
            document.getElementById('productImagePreview').style.display = 'none';
            loadProducts();
            loadDashboardStats();
            
            showSuccess(currentLanguage === 'ar' ? 
                'تمت إضافة المنتج بنجاح' : 
                'Product added successfully');
        } catch (error) {
            console.error('Error adding product:', error);
            showError(currentLanguage === 'ar' ? 
                'فشل إضافة المنتج: ' + error.message : 
                'Failed to add product: ' + error.message);
        }
    });
    
    // Image preview for edit product
    document.getElementById('editProductImage').addEventListener('change', function() {
        const preview = document.getElementById('editProductImagePreview');
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                preview.src = e.target.result;
            }
            reader.readAsDataURL(file);
        }
    });
}

// Edit product
async function editProduct(productId) {
    try {
        const response = await fetch(`https://localhost:7018/api/products/${productId}`, {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error(currentLanguage === 'ar' ? 
                'فشل تحميل بيانات المنتج' : 
                'Failed to load product details');
        }
        
        const product = await response.json();
        
        // Populate form
        document.getElementById('editProductId').value = product.id;
        document.getElementById('editProductName').value = product.name;
        document.getElementById('editProductNameAr').value = product.nameAr || '';
        document.getElementById('editProductDescription').value = product.description;
        document.getElementById('editProductDescriptionAr').value = product.descriptionAr || '';
        document.getElementById('editProductPrice').value = product.price;
        document.getElementById('editProductCategory').value = product.categoryId;
        document.getElementById('editProductIsAvailable').checked = product.isAvailable;
        
        // Show image preview
        const imagePreview = document.getElementById('editProductImagePreview');
        if (product.imageUrl) {
            imagePreview.src = product.imageUrl;
        } else {
            imagePreview.style.display = 'none';
        }
        
        // Show modal
        const modal = new bootstrap.Modal(document.getElementById('editProductModal'));
        modal.show();
        
        // Setup update button
        document.getElementById('updateProductBtn').onclick = async function() {
            const formData = new FormData();
            
            formData.append('Name', document.getElementById('editProductName').value);
            formData.append('NameAr', document.getElementById('editProductNameAr').value);
            formData.append('Description', document.getElementById('editProductDescription').value);
            formData.append('DescriptionAr', document.getElementById('editProductDescriptionAr').value);
            formData.append('Price', document.getElementById('editProductPrice').value);
            formData.append('CategoryId', document.getElementById('editProductCategory').value);
            formData.append('IsAvailable', document.getElementById('editProductIsAvailable').checked);
            
            // Only append image if a new one was selected
            const imageFile = document.getElementById('editProductImage').files[0];
            if (imageFile) {
                formData.append('Image', imageFile);
            }
            
            try {
                const updateResponse = await fetch(`https://localhost:7018/api/products/${productId}`, {
                    method: 'PUT',
                    headers: getAuthHeaders(false),
                    body: formData
                });
                
                if (!updateResponse.ok) {
                    const errorData = await updateResponse.json();
                    throw new Error(errorData.message || (currentLanguage === 'ar' ? 
                        'فشل تحديث المنتج' : 
                        'Failed to update product'));
                }
                
                // Close modal and refresh products
                modal.hide();
                loadProducts();
                loadDashboardStats();
                
                showSuccess(currentLanguage === 'ar' ? 
                    'تم تحديث المنتج بنجاح' : 
                    'Product updated successfully');
            } catch (error) {
                console.error('Error updating product:', error);
                showError(currentLanguage === 'ar' ? 
                    'فشل تحديث المنتج: ' + error.message : 
                    'Failed to update product: ' + error.message);
            }
        };
    } catch (error) {
        console.error('Error loading product for edit:', error);
        showError(currentLanguage === 'ar' ? 
            'خطأ في تحميل المنتج للتعديل' : 
            'Error loading product for editing');
    }
}

// Setup category forms
function setupCategoryForms() {
    // Image preview for add category
    document.getElementById('categoryImage').addEventListener('change', function() {
        const preview = document.getElementById('categoryImagePreview');
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                preview.src = e.target.result;
                preview.style.display = 'block';
            }
            reader.readAsDataURL(file);
        }
    });

    // Save category
    document.getElementById('saveCategoryBtn').addEventListener('click', async function() {
        const form = document.getElementById('addCategoryForm');
        const formData = new FormData();
        
        // Add all fields
        formData.append('Name', document.getElementById('categoryName').value);
        formData.append('NameAr', document.getElementById('categoryNameAr').value);
        formData.append('Description', document.getElementById('categoryDescription').value);
        formData.append('DescriptionAr', document.getElementById('categoryDescriptionAr').value);
        formData.append('Image', document.getElementById('categoryImage').files[0]);
        
        try {
            const response = await fetch('https://localhost:7018/api/categories', {
                method: 'POST',
                headers: getAuthHeaders(false),
                body: formData
            });
            
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || (currentLanguage === 'ar' ? 
                    'فشل إضافة الفئة' : 
                    'Failed to add category'));
            }
            
            // Close modal and refresh categories
            bootstrap.Modal.getInstance(document.getElementById('addCategoryModal')).hide();
            form.reset();
            document.getElementById('categoryImagePreview').style.display = 'none';
            loadCategories();
            loadDashboardStats();
            
            showSuccess(currentLanguage === 'ar' ? 
                'تمت إضافة الفئة بنجاح' : 
                'Category added successfully');
        } catch (error) {
            console.error('Error adding category:', error);
            showError(currentLanguage === 'ar' ? 
                'فشل إضافة الفئة: ' + error.message : 
                'Failed to add category: ' + error.message);
        }
    });
    
    // Image preview for edit category
    document.getElementById('editCategoryImage').addEventListener('change', function() {
        const preview = document.getElementById('editCategoryImagePreview');
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                preview.src = e.target.result;
            }
            reader.readAsDataURL(file);
        }
    });
}


// Edit category
async function editCategory(categoryId) {
    try {
        const response = await fetch(`https://localhost:7018/api/categories/${categoryId}`, {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error(currentLanguage === 'ar' ? 
                'فشل تحميل بيانات الفئة' : 
                'Failed to load category details');
        }
        
        const category = await response.json();
        
        // Populate form
        document.getElementById('editCategoryId').value = category.id;
        document.getElementById('editCategoryName').value = category.name;
        document.getElementById('editCategoryNameAr').value = category.nameAr || '';
        document.getElementById('editCategoryDescription').value = category.description;
        document.getElementById('editCategoryDescriptionAr').value = category.descriptionAr || '';
        
        // Show image preview
        const imagePreview = document.getElementById('editCategoryImagePreview');
        if (category.imageUrl) {
            imagePreview.src = category.imageUrl;
            imagePreview.style.display = 'block';
        } else {
            imagePreview.style.display = 'none';
        }
        
        // Show modal
        const modal = new bootstrap.Modal(document.getElementById('editCategoryModal'));
        modal.show();
        
        // Setup update button
        document.getElementById('updateCategoryBtn').onclick = async function() {
            const formData = new FormData();
            
            formData.append('Name', document.getElementById('editCategoryName').value);
            formData.append('NameAr', document.getElementById('editCategoryNameAr').value);
            formData.append('Description', document.getElementById('editCategoryDescription').value);
            formData.append('DescriptionAr', document.getElementById('editCategoryDescriptionAr').value);
            
            // Only append image if a new one was selected
            const imageFile = document.getElementById('editCategoryImage').files[0];
            if (imageFile) {
                formData.append('Image', imageFile);
            }
            
            try {
                const updateResponse = await fetch(`https://localhost:7018/api/categories/${categoryId}`, {
                    method: 'PUT',
                    headers: getAuthHeaders(false),
                    body: formData
                });
                
                if (!updateResponse.ok) {
                    const errorData = await updateResponse.json();
                    throw new Error(errorData.message || (currentLanguage === 'ar' ? 
                        'فشل تحديث الفئة' : 
                        'Failed to update category'));
                }
                
                // Close modal and refresh categories
                modal.hide();
                loadCategories();
                loadDashboardStats();
                
                showSuccess(currentLanguage === 'ar' ? 
                    'تم تحديث الفئة بنجاح' : 
                    'Category updated successfully');
            } catch (error) {
                console.error('Error updating category:', error);
                showError(currentLanguage === 'ar' ? 
                    'فشل تحديث الفئة: ' + error.message : 
                    'Failed to update category: ' + error.message);
            }
        };
    } catch (error) {
        console.error('Error loading category for edit:', error);
        showError(currentLanguage === 'ar' ? 
            'خطأ في تحميل الفئة للتعديل' : 
            'Error loading category for editing');
    }
}

// Delete category
async function deleteCategory(categoryId) {
    try {
        const response = await fetch(`https://localhost:7018/api/categories/${categoryId}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error(currentLanguage === 'ar' ? 
                'فشل حذف الفئة' : 
                'Failed to delete category');
        }
        
        // Refresh categories
        loadCategories();
        loadDashboardStats();
        
        showSuccess(currentLanguage === 'ar' ? 
            'تم حذف الفئة بنجاح' : 
            'Category deleted successfully');
    } catch (error) {
        console.error('Error deleting category:', error);
        showError(currentLanguage === 'ar' ? 
            'فشل حذف الفئة' : 
            'Failed to delete category');
    }
}

function setupImagePreviews() {
    // Product image preview for add modal
    document.getElementById('productImage').addEventListener('change', function() {
        const preview = document.getElementById('productImagePreview');
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                preview.src = e.target.result;
                preview.style.display = 'block';
            }
            reader.readAsDataURL(file);
        }
    });

    // Category image preview for add modal
    document.getElementById('categoryImage').addEventListener('change', function() {
        const preview = document.getElementById('categoryImagePreview');
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                preview.src = e.target.result;
                preview.style.display = 'block';
            }
            reader.readAsDataURL(file);
        }
    });
}

// Helper function to get auth headers
function getAuthHeaders(includeContentType = true) {
    const token = localStorage.getItem('adminToken');
    const headers = {};
    
    if (includeContentType) {
        headers['Content-Type'] = 'application/json';
    }
    
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    
    return headers;
}

// Show success message
function showSuccess(message) {
    // You can replace this with a proper toast notification
    alert(message);
}

// Show error message
function showError(message) {
    // You can replace this with a proper toast notification
    alert(message);
}