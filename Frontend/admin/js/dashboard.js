// Check authentication
document.addEventListener('DOMContentLoaded', function() {
    const token = localStorage.getItem('adminToken');
    if (!token) {
        window.location.href = '/admin/login.html';
        return;
    }
    
    // Load initial data
    loadDashboardStats();
    loadProducts();
    loadCategories();
    
    // Setup navigation
    setupNavigation();
    
    // Setup logout button
    document.getElementById('logoutBtn').addEventListener('click', function() {
        localStorage.removeItem('adminToken');
        window.location.href = '/admin/login.html';
    });
    
    // Setup product form
    setupProductForm();
    
    // Setup category form
    setupCategoryForm();
});

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
            throw new Error('Failed to load dashboard stats');
        }
        
        const products = await productsResponse.json();
        const categories = await categoriesResponse.json();
        
        document.getElementById('totalProducts').textContent = products.length;
        document.getElementById('totalCategories').textContent = categories.length;
    } catch (error) {
        console.error('Error loading dashboard stats:', error);
        alert('Failed to load dashboard data');
    }
}

async function loadProducts() {
    try {
        const response = await fetch('https://localhost:7018/api/products', {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error('Failed to load products');
        }
        
        const products = await response.json();
        const tableBody = document.getElementById('productsTableBody');
        tableBody.innerHTML = '';
        
        products.forEach(product => {
            const row = document.createElement('tr');
            // In the products table row creation
row.innerHTML = `
    <td>${product.id}</td>
    <td>
        ${product.imageUrl ? `<img src="${product.imageUrl}" alt="${product.name}" style="max-height: 50px; margin-right: 10px;">` : ''}
        ${product.name}
    </td>
    <td>$${product.price.toFixed(2)}</td>
    <td>${product.categoryName || 'N/A'}</td>
    <td>${product.isAvailable ? '<span class="badge bg-success">Available</span>' : '<span class="badge bg-danger">Unavailable</span>'}</td>
    <td>
        <button class="btn btn-sm btn-primary edit-product" data-id="${product.id}">Edit</button>
        <button class="btn btn-sm btn-danger delete-product" data-id="${product.id}">Delete</button>
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
                if (confirm('Are you sure you want to delete this product?')) {
                    deleteProduct(productId);
                }
            });
        });
    } catch (error) {
        console.error('Error loading products:', error);
        alert('Failed to load products');
    }
}

async function loadCategories() {
    try {
        const response = await fetch('https://localhost:7018/api/categories', {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error('Failed to load categories');
        }
        
        const categories = await response.json();
        const tableBody = document.getElementById('categoriesTableBody');
        tableBody.innerHTML = '';
        
        categories.forEach(category => {
            const row = document.createElement('tr');
            // In the categories table row creation
row.innerHTML = `
    <td>${category.id}</td>
    <td>
        ${category.imageUrl ? `<img src="${category.imageUrl}" alt="${category.name}" style="max-height: 50px; margin-right: 10px;">` : ''}
        ${category.name}
    </td>
    <td>${category.description}</td>
    <td>
        <button class="btn btn-sm btn-primary edit-category" data-id="${category.id}">Edit</button>
        <button class="btn btn-sm btn-danger delete-category" data-id="${category.id}">Delete</button>
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
                if (confirm('Are you sure you want to delete this category?')) {
                    deleteCategory(categoryId);
                }
            });
        });
        
        // Also populate category dropdowns in product forms
        populateCategoryDropdowns(categories);
    } catch (error) {
        console.error('Error loading categories:', error);
        alert('Failed to load categories');
    }
}

function populateCategoryDropdowns(categories) {
    const productCategory = document.getElementById('productCategory');
    const editProductCategory = document.getElementById('editProductCategory');
    
    productCategory.innerHTML = '';
    editProductCategory.innerHTML = '';
    
    categories.forEach(category => {
        const option = document.createElement('option');
        option.value = category.id;
        option.textContent = category.name;
        
        productCategory.appendChild(option.cloneNode(true));
        editProductCategory.appendChild(option.cloneNode(true));
    });
}

function setupProductForm() {
    document.getElementById('saveProductBtn').addEventListener('click', async function() {
        const form = document.getElementById('addProductForm');
        const formData = new FormData();
        
        formData.append('Name', document.getElementById('productName').value);
        formData.append('Description', document.getElementById('productDescription').value);
        formData.append('Price', document.getElementById('productPrice').value);
        formData.append('CategoryId', document.getElementById('productCategory').value);
        formData.append('IsAvailable', document.getElementById('productIsAvailable').checked);
        formData.append('Image', document.getElementById('productImage').files[0]);
        
        try {
            const response = await fetch('https://localhost:7018/api/products', {
                method: 'POST',
                headers: getAuthHeaders(false),
                body: formData
            });
            
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Failed to add product');
            }
            
            // Close modal and refresh products
            bootstrap.Modal.getInstance(document.getElementById('addProductModal')).hide();
            form.reset();
            loadProducts();
            loadDashboardStats();
        } catch (error) {
            console.error('Error adding product:', error);
            alert('Failed to add product: ' + error.message);
        }
    });
}

function setupCategoryForm() {
    document.getElementById('saveCategoryBtn').addEventListener('click', async function() {
        const form = document.getElementById('addCategoryForm');
        const formData = new FormData();
        
        formData.append('Name', document.getElementById('categoryName').value);
        formData.append('Description', document.getElementById('categoryDescription').value);
        formData.append('Image', document.getElementById('categoryImage').files[0]);
        
        try {
            const response = await fetch('https://localhost:7018/api/categories', {
                method: 'POST',
                headers: getAuthHeaders(false),
                body: formData
            });
            
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Failed to add category');
            }
            
            // Close modal and refresh categories
            bootstrap.Modal.getInstance(document.getElementById('addCategoryModal')).hide();
            form.reset();
            loadCategories();
            loadDashboardStats();
        } catch (error) {
            console.error('Error adding category:', error);
            alert('Failed to add category: ' + error.message);
        }
    });
}

async function editProduct(productId) {
    const API_BASE_URL = 'https://localhost:7018';
    
    try {
        // Load product data
        const response = await fetch(`${API_BASE_URL}/api/products/${productId}`, {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) throw new Error('Failed to load product');
        
        const product = await response.json();
        
        // Populate form
        document.getElementById('editProductId').value = product.id;
        document.getElementById('editProductName').value = product.name;
        document.getElementById('editProductDescription').value = product.description;
        document.getElementById('editProductPrice').value = product.price;
        document.getElementById('editProductIsAvailable').checked = product.isAvailable;
        
        // Set category
        const categorySelect = document.getElementById('editProductCategory');
        for (let i = 0; i < categorySelect.options.length; i++) {
            if (categorySelect.options[i].value == product.categoryId) {
                categorySelect.selectedIndex = i;
                break;
            }
        }
        
        // Show image preview
        const imagePreview = document.getElementById('editProductImagePreview');
        if (product.imageUrl) {
            imagePreview.src = product.imageUrl;
            imagePreview.style.display = 'block';
        } else {
            imagePreview.style.display = 'none';
        }
        
        // Show modal
        const modal = new bootstrap.Modal(document.getElementById('editProductModal'));
        modal.show();
        
        // Update button handler - remove previous listener to avoid duplicates
        const updateBtn = document.getElementById('updateProductBtn');
        updateBtn.onclick = null; // Clear previous handler
        updateBtn.onclick = async function() {
            const formData = new FormData();
            
            // Add all fields except image
            formData.append('Name', document.getElementById('editProductName').value);
            formData.append('Description', document.getElementById('editProductDescription').value);
            formData.append('Price', document.getElementById('editProductPrice').value);
            formData.append('CategoryId', document.getElementById('editProductCategory').value);
            formData.append('IsAvailable', document.getElementById('editProductIsAvailable').checked);
            
            // Only append image if a new one was selected
            const imageFile = document.getElementById('editProductImage').files[0];
            if (imageFile) {
    formData.append('Image', imageFile);
} else {
    formData.append('ImageUrl', document.getElementById('editProductImageExisting').value);
}

            
            try {
                const updateResponse = await fetch(`${API_BASE_URL}/api/products/${productId}`, {
                    method: 'PUT',
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('adminToken')}`
                    },
                    body: formData
                });
                
                if (!updateResponse.ok) {
                    let errorMessage = 'Failed to update product';
                    try {
                        const errorData = await updateResponse.text();
                        try {
                            const jsonError = JSON.parse(errorData);
                            errorMessage = jsonError.message || errorMessage;
                        } catch {
                            errorMessage = errorData || errorMessage;
                        }
                    } catch (e) {
                        errorMessage = updateResponse.statusText || errorMessage;
                    }
                    throw new Error(errorMessage);
                }
                
                modal.hide();
                loadProducts();
                loadDashboardStats();
            } catch (error) {
                console.error('Update error:', error);
                alert(`Update failed: ${error.message}`);
            }
        };
    } catch (error) {
        console.error('Edit product error:', error);
        alert(`Failed to load product: ${error.message}`);
    }
}



async function editCategory(categoryId) {
    try {
        const response = await fetch(`https://localhost:7018/api/categories/${categoryId}`, {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error('Failed to load category details');
        }
        
        const category = await response.json();
        
        // Populate form
        document.getElementById('editCategoryId').value = category.id;
        document.getElementById('editCategoryName').value = category.name;
        document.getElementById('editCategoryDescription').value = category.description;
        
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
            formData.append('Description', document.getElementById('editCategoryDescription').value);
            
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
                    throw new Error(errorData.message || 'Failed to update category');
                }
                
                // Close modal and refresh categories
                modal.hide();
                loadCategories();
                loadDashboardStats();
            } catch (error) {
                console.error('Error updating category:', error);
                alert('Failed to update category: ' + error.message);
            }
        };
    } catch (error) {
        console.error('Error loading category for edit:', error);
        alert('Failed to load category details');
    }
}

async function deleteProduct(productId) {
    try {
        const response = await fetch(`https://localhost:7018/api/products/${productId}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error('Failed to delete product');
        }
        
        // Refresh products
        loadProducts();
        loadDashboardStats();
    } catch (error) {
        console.error('Error deleting product:', error);
        alert('Failed to delete product');
    }
}

async function deleteCategory(categoryId) {
    try {
        const response = await fetch(`https://localhost:7018/api/categories/${categoryId}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error('Failed to delete category');
        }
        
        // Refresh categories
        loadCategories();
        loadDashboardStats();
    } catch (error) {
        console.error('Error deleting category:', error);
        alert('Failed to delete category');
    }
}

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