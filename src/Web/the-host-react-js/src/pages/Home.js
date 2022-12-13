import { useSelector } from 'react-redux'
import Translation from './../components/Translation'
// import ReactCrop from 'react-image-crop/dist/ReactCrop.min.js'
// import 'react-image-crop/dist/ReactCrop.css'
// import { useState } from 'react'

function Home() {
    const isLoggedIn = useSelector((state) => state.auth.isLoggedIn)
    const fullname = useSelector((state) => state.user.name + ' ' + state.user.surname)

    // const [crop, setCrop] = useState();
    // const [src, setSrc] = useState();

    return (
        <div className="container-lg d-flex flex-column pt-2">
            {
                isLoggedIn ? <Translation msg="WELCOME" /> + ", " + fullname : null
            }

            {/* <ReactCrop crop={crop} onChange={(c) => setCrop(c)}>
                <img src="https://tugkanmeral.com/pictures/merhum/eb8c2c76-876f-4d3f-bd7c-bdfb010ded2c.png" />
            </ReactCrop>

            <button onClick={() => console.log(crop)}>log</button> */}
        </div>
    )
}

export default Home;