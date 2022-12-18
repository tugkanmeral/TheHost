import { FaKey, FaPen, FaTools } from 'react-icons/fa'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
// import ReactCrop from 'react-image-crop/dist/ReactCrop.min.js'
// import 'react-image-crop/dist/ReactCrop.css'
// import { useState } from 'react'

function Home() {
    const isLoggedIn = useSelector((state) => state.auth.isLoggedIn)

    const navigate = useNavigate();
    // const [crop, setCrop] = useState();
    // const [src, setSrc] = useState();

    return (
        <div className="container-lg d-flex flex-column pt-2 justify-content-center" style={{ height: '100vh' }}>
            {
                isLoggedIn ?
                    <div className='d-inline-flex justify-content-around'>
                        <FaKey size={55} color='rgb(75,75,75)' onClick={() => navigate('/password')} style={{ cursor: 'pointer' }} />
                        <FaPen size={55} color='rgb(75,75,75)' onClick={() => navigate('/note')} style={{ cursor: 'pointer' }} />
                        <FaTools size={55} color='rgb(75,75,75)' onClick={() => navigate('/tool')} style={{ cursor: 'pointer' }} />
                    </div>
                    : null
            }

            {/* <ReactCrop crop={crop} onChange={(c) => setCrop(c)}>
                <img src="https://tugkanmeral.com/pictures/merhum/eb8c2c76-876f-4d3f-bd7c-bdfb010ded2c.png" />
            </ReactCrop>

            <button onClick={() => console.log(crop)}>log</button> */}
        </div>
    )
}

export default Home;